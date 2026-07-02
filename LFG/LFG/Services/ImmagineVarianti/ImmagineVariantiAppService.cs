using LFG.Shared;
using LFG.VarianteProdotti;
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
using LFG.ImmagineVarianti;
using MiniExcelLibs;
using Volo.Abp.Content;
using Volo.Abp.Authorization;
using Volo.Abp.Caching;
using Microsoft.Extensions.Caching.Distributed;
using LFG.Shared;

namespace LFG.ImmagineVarianti;

[Authorize(LFGPermissions.ImmagineVarianti.Default)]
public abstract class ImmagineVariantiAppServiceBase : ApplicationService
{
    protected IDistributedCache<ImmagineVarianteDownloadTokenCacheItem, string> _downloadTokenCache;
    protected IImmagineVarianteRepository _immagineVarianteRepository;
    protected ImmagineVarianteManager _immagineVarianteManager;
    protected IRepository<LFG.VarianteProdotti.VarianteProdotto, Guid> _varianteProdottoRepository;

    public ImmagineVariantiAppServiceBase(IImmagineVarianteRepository immagineVarianteRepository, ImmagineVarianteManager immagineVarianteManager, IDistributedCache<ImmagineVarianteDownloadTokenCacheItem, string> downloadTokenCache, IRepository<LFG.VarianteProdotti.VarianteProdotto, Guid> varianteProdottoRepository)
    {
        _downloadTokenCache = downloadTokenCache;
        _immagineVarianteRepository = immagineVarianteRepository;
        _immagineVarianteManager = immagineVarianteManager;
        _varianteProdottoRepository = varianteProdottoRepository;
    }

    public virtual async Task<PagedResultDto<ImmagineVarianteWithNavigationPropertiesDto>> GetListAsync(GetImmagineVariantiInput input)
    {
        var totalCount = await _immagineVarianteRepository.GetCountAsync(input.FilterText, input.VarianteProdottoId, input.Url, input.OrdineMin, input.OrdineMax);
        var items = await _immagineVarianteRepository.GetListWithNavigationPropertiesAsync(input.FilterText, input.VarianteProdottoId, input.Url, input.OrdineMin, input.OrdineMax, input.Sorting, input.MaxResultCount, input.SkipCount);
        return new PagedResultDto<ImmagineVarianteWithNavigationPropertiesDto>
        {
            TotalCount = totalCount,
            Items = ObjectMapper.Map<List<ImmagineVarianteWithNavigationProperties>, List<ImmagineVarianteWithNavigationPropertiesDto>>(items)
        };
    }

    public virtual async Task<ImmagineVarianteWithNavigationPropertiesDto> GetWithNavigationPropertiesAsync(Guid id)
    {
        return ObjectMapper.Map<ImmagineVarianteWithNavigationProperties, ImmagineVarianteWithNavigationPropertiesDto>(await _immagineVarianteRepository.GetWithNavigationPropertiesAsync(id));
    }

    public virtual async Task<ImmagineVarianteDto> GetAsync(Guid id)
    {
        return ObjectMapper.Map<ImmagineVariante, ImmagineVarianteDto>(await _immagineVarianteRepository.GetAsync(id));
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

    [Authorize(LFGPermissions.ImmagineVarianti.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _immagineVarianteRepository.DeleteAsync(id);
    }

    [Authorize(LFGPermissions.ImmagineVarianti.Create)]
    public virtual async Task<ImmagineVarianteDto> CreateAsync(ImmagineVarianteCreateDto input)
    {
        var immagineVariante = await _immagineVarianteManager.CreateAsync(input.VarianteProdottoId, input.Url, input.Ordine);
        return ObjectMapper.Map<ImmagineVariante, ImmagineVarianteDto>(immagineVariante);
    }

    [Authorize(LFGPermissions.ImmagineVarianti.Edit)]
    public virtual async Task<ImmagineVarianteDto> UpdateAsync(Guid id, ImmagineVarianteUpdateDto input)
    {
        var immagineVariante = await _immagineVarianteManager.UpdateAsync(id, input.VarianteProdottoId, input.Url, input.Ordine, input.ConcurrencyStamp);
        return ObjectMapper.Map<ImmagineVariante, ImmagineVarianteDto>(immagineVariante);
    }

    [AllowAnonymous]
    public virtual async Task<IRemoteStreamContent> GetListAsExcelFileAsync(ImmagineVarianteExcelDownloadDto input)
    {
        var downloadToken = await _downloadTokenCache.GetAsync(input.DownloadToken);
        if (downloadToken == null || input.DownloadToken != downloadToken.Token)
        {
            throw new AbpAuthorizationException("Invalid download token: " + input.DownloadToken);
        }

        var immagineVarianti = await _immagineVarianteRepository.GetListWithNavigationPropertiesAsync(input.FilterText, input.VarianteProdottoId, input.Url, input.OrdineMin, input.OrdineMax);
        var items = immagineVarianti.Select(item => new { VarianteProdottoId = item.ImmagineVariante.VarianteProdottoId, Url = item.ImmagineVariante.Url, Ordine = item.ImmagineVariante.Ordine, VarianteProdotto = item.VarianteProdotto?.UrlImmagine, });
        var memoryStream = new MemoryStream();
        await memoryStream.SaveAsAsync(items);
        memoryStream.Seek(0, SeekOrigin.Begin);
        return new RemoteStreamContent(memoryStream, "ImmagineVarianti.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
    }

    [Authorize(LFGPermissions.ImmagineVarianti.Delete)]
    public virtual async Task DeleteByIdsAsync(List<Guid> immaginevarianteIds)
    {
        await _immagineVarianteRepository.DeleteManyAsync(immaginevarianteIds);
    }

    [Authorize(LFGPermissions.ImmagineVarianti.Delete)]
    public virtual async Task DeleteAllAsync(GetImmagineVariantiInput input)
    {
        await _immagineVarianteRepository.DeleteAllAsync(input.FilterText, input.VarianteProdottoId, input.Url, input.OrdineMin, input.OrdineMax);
    }

    public virtual async Task<LFG.Shared.DownloadTokenResultDto> GetDownloadTokenAsync()
    {
        var token = Guid.NewGuid().ToString("N");
        await _downloadTokenCache.SetAsync(token, new ImmagineVarianteDownloadTokenCacheItem { Token = token }, new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30) });
        return new LFG.Shared.DownloadTokenResultDto
        {
            Token = token
        };
    }
}