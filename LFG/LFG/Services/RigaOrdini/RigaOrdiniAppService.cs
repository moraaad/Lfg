using LFG.Shared;
using LFG.VarianteProdotti;
using LFG.Ordini;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using LFG.Permissions;
using LFG.RigaOrdini;
using MiniExcelLibs;
using Volo.Abp.Content;
using Volo.Abp.Authorization;
using Volo.Abp.Caching;
using Microsoft.Extensions.Caching.Distributed;
using LFG.Shared;

namespace LFG.RigaOrdini;

[Authorize(LFGPermissions.RigaOrdini.Default)]
public abstract class RigaOrdiniAppServiceBase : ApplicationService
{
    protected IDistributedCache<RigaOrdineDownloadTokenCacheItem, string> _downloadTokenCache;
    protected IRigaOrdineRepository _rigaOrdineRepository;
    protected RigaOrdineManager _rigaOrdineManager;
    protected IRepository<LFG.Ordini.Ordine, Guid> _ordineRepository;
    protected IRepository<LFG.VarianteProdotti.VarianteProdotto, Guid> _varianteProdottoRepository;

    public RigaOrdiniAppServiceBase(IRigaOrdineRepository rigaOrdineRepository, RigaOrdineManager rigaOrdineManager, IDistributedCache<RigaOrdineDownloadTokenCacheItem, string> downloadTokenCache, IRepository<LFG.Ordini.Ordine, Guid> ordineRepository, IRepository<LFG.VarianteProdotti.VarianteProdotto, Guid> varianteProdottoRepository)
    {
        _downloadTokenCache = downloadTokenCache;
        _rigaOrdineRepository = rigaOrdineRepository;
        _rigaOrdineManager = rigaOrdineManager;
        _ordineRepository = ordineRepository;
        _varianteProdottoRepository = varianteProdottoRepository;
    }

    public virtual async Task<PagedResultDto<RigaOrdineWithNavigationPropertiesDto>> GetListAsync(GetRigaOrdiniInput input)
    {
        var totalCount = await _rigaOrdineRepository.GetCountAsync(input.FilterText, input.QuantitaMin, input.QuantitaMax, input.PrezzoUnitarioMin, input.PrezzoUnitarioMax, input.OrdineId, input.VarianteProdottoId);
        var items = await _rigaOrdineRepository.GetListWithNavigationPropertiesAsync(input.FilterText, input.QuantitaMin, input.QuantitaMax, input.PrezzoUnitarioMin, input.PrezzoUnitarioMax, input.OrdineId, input.VarianteProdottoId, input.Sorting, input.MaxResultCount, input.SkipCount);
        return new PagedResultDto<RigaOrdineWithNavigationPropertiesDto>
        {
            TotalCount = totalCount,
            Items = ObjectMapper.Map<List<RigaOrdineWithNavigationProperties>, List<RigaOrdineWithNavigationPropertiesDto>>(items)
        };
    }

    public virtual async Task<RigaOrdineWithNavigationPropertiesDto> GetWithNavigationPropertiesAsync(Guid id)
    {
        return ObjectMapper.Map<RigaOrdineWithNavigationProperties, RigaOrdineWithNavigationPropertiesDto>(await _rigaOrdineRepository.GetWithNavigationPropertiesAsync(id));
    }

    public virtual async Task<RigaOrdineDto> GetAsync(Guid id)
    {
        return ObjectMapper.Map<RigaOrdine, RigaOrdineDto>(await _rigaOrdineRepository.GetAsync(id));
    }

    public virtual async Task<PagedResultDto<LookupDto<Guid>>> GetOrdineLookupAsync(LookupRequestDto input)
    {
        var query = (await _ordineRepository.GetQueryableAsync()).WhereIf(!string.IsNullOrWhiteSpace(input.Filter), x => x.IndSpedizione != null && x.IndSpedizione.Contains(input.Filter));
        var lookupData = await query.PageBy(input.SkipCount, input.MaxResultCount).ToDynamicListAsync<LFG.Ordini.Ordine>();
        var totalCount = query.Count();
        return new PagedResultDto<LookupDto<Guid>>
        {
            TotalCount = totalCount,
            Items = lookupData.Select(x => new LookupDto<Guid> { Id = x.Id, DisplayName = x.IndSpedizione }).ToList()
        };
    }

    public virtual async Task<PagedResultDto<LookupDto<Guid>>> GetVarianteProdottoLookupAsync(LookupRequestDto input)
    {
        var query = (await _varianteProdottoRepository.GetQueryableAsync()).WhereIf(!string.IsNullOrWhiteSpace(input.Filter), x => x.UrlImmagine != null && x.UrlImmagine.Contains(input.Filter));
        var lookupData = await query.PageBy(input.SkipCount, input.MaxResultCount).ToDynamicListAsync<LFG.VarianteProdotti.VarianteProdotto>();
        var totalCount = query.Count();
        return new PagedResultDto<LookupDto<Guid>>
        {
            TotalCount = totalCount,
            Items = lookupData.Select(x => new LookupDto<Guid> { Id = x.Id, DisplayName = x.UrlImmagine }).ToList()
        };
    }

    [Authorize(LFGPermissions.RigaOrdini.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _rigaOrdineRepository.DeleteAsync(id);
    }

    [Authorize(LFGPermissions.RigaOrdini.Create)]
    public virtual async Task<RigaOrdineDto> CreateAsync(RigaOrdineCreateDto input)
    {
        var rigaOrdine = await _rigaOrdineManager.CreateAsync(input.OrdineId, input.VarianteProdottoId, input.Quantita, input.PrezzoUnitario);
        return ObjectMapper.Map<RigaOrdine, RigaOrdineDto>(rigaOrdine);
    }

    [Authorize(LFGPermissions.RigaOrdini.Edit)]
    public virtual async Task<RigaOrdineDto> UpdateAsync(Guid id, RigaOrdineUpdateDto input)
    {
        var rigaOrdine = await _rigaOrdineManager.UpdateAsync(id, input.OrdineId, input.VarianteProdottoId, input.Quantita, input.PrezzoUnitario, input.ConcurrencyStamp);
        return ObjectMapper.Map<RigaOrdine, RigaOrdineDto>(rigaOrdine);
    }

    [AllowAnonymous]
    public virtual async Task<IRemoteStreamContent> GetListAsExcelFileAsync(RigaOrdineExcelDownloadDto input)
    {
        var downloadToken = await _downloadTokenCache.GetAsync(input.DownloadToken);
        if (downloadToken == null || input.DownloadToken != downloadToken.Token)
        {
            throw new AbpAuthorizationException("Invalid download token: " + input.DownloadToken);
        }

        var rigaOrdini = await _rigaOrdineRepository.GetListWithNavigationPropertiesAsync(input.FilterText, input.QuantitaMin, input.QuantitaMax, input.PrezzoUnitarioMin, input.PrezzoUnitarioMax, input.OrdineId, input.VarianteProdottoId);
        var items = rigaOrdini.Select(item => new { Quantita = item.RigaOrdine.Quantita, PrezzoUnitario = item.RigaOrdine.PrezzoUnitario, Ordine = item.Ordine?.IndSpedizione, VarianteProdotto = item.VarianteProdotto?.UrlImmagine, });
        var memoryStream = new MemoryStream();
        await memoryStream.SaveAsAsync(items);
        memoryStream.Seek(0, SeekOrigin.Begin);
        return new RemoteStreamContent(memoryStream, "RigaOrdini.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
    }

    [Authorize(LFGPermissions.RigaOrdini.Delete)]
    public virtual async Task DeleteByIdsAsync(List<Guid> rigaordineIds)
    {
        await _rigaOrdineRepository.DeleteManyAsync(rigaordineIds);
    }

    [Authorize(LFGPermissions.RigaOrdini.Delete)]
    public virtual async Task DeleteAllAsync(GetRigaOrdiniInput input)
    {
        await _rigaOrdineRepository.DeleteAllAsync(input.FilterText, input.QuantitaMin, input.QuantitaMax, input.PrezzoUnitarioMin, input.PrezzoUnitarioMax, input.OrdineId, input.VarianteProdottoId);
    }

    public virtual async Task<LFG.Shared.DownloadTokenResultDto> GetDownloadTokenAsync()
    {
        var token = Guid.NewGuid().ToString("N");
        await _downloadTokenCache.SetAsync(token, new RigaOrdineDownloadTokenCacheItem { Token = token }, new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30) });
        return new LFG.Shared.DownloadTokenResultDto
        {
            Token = token
        };
    }
}