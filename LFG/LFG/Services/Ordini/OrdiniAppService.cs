using LFG.Shared;
using LFG.Sconti;
using LFG.Clienti;
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
using LFG.Ordini;
using MiniExcelLibs;
using Volo.Abp.Content;
using Volo.Abp.Authorization;
using Volo.Abp.Caching;
using Microsoft.Extensions.Caching.Distributed;
using LFG.Shared;

namespace LFG.Ordini;

[Authorize(LFGPermissions.Ordini.Default)]
public abstract class OrdiniAppServiceBase : ApplicationService
{
    protected IDistributedCache<OrdineDownloadTokenCacheItem, string> _downloadTokenCache;
    protected IOrdineRepository _ordineRepository;
    protected OrdineManager _ordineManager;
    protected IRepository<LFG.Clienti.Cliente, Guid> _clienteRepository;
    protected IRepository<LFG.Sconti.Sconto, Guid> _scontoRepository;

    public OrdiniAppServiceBase(IOrdineRepository ordineRepository, OrdineManager ordineManager, IDistributedCache<OrdineDownloadTokenCacheItem, string> downloadTokenCache, IRepository<LFG.Clienti.Cliente, Guid> clienteRepository, IRepository<LFG.Sconti.Sconto, Guid> scontoRepository)
    {
        _downloadTokenCache = downloadTokenCache;
        _ordineRepository = ordineRepository;
        _ordineManager = ordineManager;
        _clienteRepository = clienteRepository;
        _scontoRepository = scontoRepository;
    }

    public virtual async Task<PagedResultDto<OrdineWithNavigationPropertiesDto>> GetListAsync(GetOrdiniInput input)
    {
        var totalCount = await _ordineRepository.GetCountAsync(input.FilterText, input.DataOrdineMin, input.DataOrdineMax, input.Stato, input.ImportoTotaleMin, input.ImportoTotaleMax, input.IndSpedizione, input.MetodoPagamento, input.ClienteId, input.ScontoId);
        var items = await _ordineRepository.GetListWithNavigationPropertiesAsync(input.FilterText, input.DataOrdineMin, input.DataOrdineMax, input.Stato, input.ImportoTotaleMin, input.ImportoTotaleMax, input.IndSpedizione, input.MetodoPagamento, input.ClienteId, input.ScontoId, input.Sorting, input.MaxResultCount, input.SkipCount);
        return new PagedResultDto<OrdineWithNavigationPropertiesDto>
        {
            TotalCount = totalCount,
            Items = ObjectMapper.Map<List<OrdineWithNavigationProperties>, List<OrdineWithNavigationPropertiesDto>>(items)
        };
    }

    public virtual async Task<OrdineWithNavigationPropertiesDto> GetWithNavigationPropertiesAsync(Guid id)
    {
        return ObjectMapper.Map<OrdineWithNavigationProperties, OrdineWithNavigationPropertiesDto>(await _ordineRepository.GetWithNavigationPropertiesAsync(id));
    }

    public virtual async Task<OrdineDto> GetAsync(Guid id)
    {
        return ObjectMapper.Map<Ordine, OrdineDto>(await _ordineRepository.GetAsync(id));
    }

    public virtual async Task<PagedResultDto<LookupDto<Guid>>> GetClienteLookupAsync(LookupRequestDto input)
    {
        var query = (await _clienteRepository.GetQueryableAsync()).WhereIf(!string.IsNullOrWhiteSpace(input.Filter), x => x.Email != null && x.Email.Contains(input.Filter));
        var lookupData = await query.PageBy(input.SkipCount, input.MaxResultCount).ToDynamicListAsync<LFG.Clienti.Cliente>();
        var totalCount = query.Count();
        return new PagedResultDto<LookupDto<Guid>>
        {
            TotalCount = totalCount,
            Items = lookupData.Select(x => new LookupDto<Guid> { Id = x.Id, DisplayName = x.Email }).ToList()
        };
    }

    public virtual async Task<PagedResultDto<LookupDto<Guid>>> GetScontoLookupAsync(LookupRequestDto input)
    {
        var query = (await _scontoRepository.GetQueryableAsync()).WhereIf(!string.IsNullOrWhiteSpace(input.Filter), x => x.Codice != null && x.Codice.Contains(input.Filter));
        var lookupData = await query.PageBy(input.SkipCount, input.MaxResultCount).ToDynamicListAsync<LFG.Sconti.Sconto>();
        var totalCount = query.Count();
        return new PagedResultDto<LookupDto<Guid>>
        {
            TotalCount = totalCount,
            Items = lookupData.Select(x => new LookupDto<Guid> { Id = x.Id, DisplayName = x.Codice }).ToList()
        };
    }

    [Authorize(LFGPermissions.Ordini.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _ordineRepository.DeleteAsync(id);
    }

    [Authorize(LFGPermissions.Ordini.Create)]
    public virtual async Task<OrdineDto> CreateAsync(OrdineCreateDto input)
    {
        var ordine = await _ordineManager.CreateAsync(input.ClienteId, input.ScontoId, input.DataOrdine, input.ImportoTotale, input.Stato, input.IndSpedizione, input.MetodoPagamento);
        return ObjectMapper.Map<Ordine, OrdineDto>(ordine);
    }

    [Authorize(LFGPermissions.Ordini.Edit)]
    public virtual async Task<OrdineDto> UpdateAsync(Guid id, OrdineUpdateDto input)
    {
        var ordine = await _ordineManager.UpdateAsync(id, input.ClienteId, input.ScontoId, input.DataOrdine, input.ImportoTotale, input.Stato, input.IndSpedizione, input.MetodoPagamento, input.ConcurrencyStamp);
        return ObjectMapper.Map<Ordine, OrdineDto>(ordine);
    }

    [AllowAnonymous]
    public virtual async Task<IRemoteStreamContent> GetListAsExcelFileAsync(OrdineExcelDownloadDto input)
    {
        var downloadToken = await _downloadTokenCache.GetAsync(input.DownloadToken);
        if (downloadToken == null || input.DownloadToken != downloadToken.Token)
        {
            throw new AbpAuthorizationException("Invalid download token: " + input.DownloadToken);
        }

        var ordini = await _ordineRepository.GetListWithNavigationPropertiesAsync(input.FilterText, input.DataOrdineMin, input.DataOrdineMax, input.Stato, input.ImportoTotaleMin, input.ImportoTotaleMax, input.IndSpedizione, input.MetodoPagamento, input.ClienteId, input.ScontoId);
        var items = ordini.Select(item => new { DataOrdine = item.Ordine.DataOrdine, Stato = item.Ordine.Stato, ImportoTotale = item.Ordine.ImportoTotale, IndSpedizione = item.Ordine.IndSpedizione, MetodoPagamento = item.Ordine.MetodoPagamento, Cliente = item.Cliente?.Email, Sconto = item.Sconto?.Codice, });
        var memoryStream = new MemoryStream();
        await memoryStream.SaveAsAsync(items);
        memoryStream.Seek(0, SeekOrigin.Begin);
        return new RemoteStreamContent(memoryStream, "Ordini.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
    }

    [Authorize(LFGPermissions.Ordini.Delete)]
    public virtual async Task DeleteByIdsAsync(List<Guid> ordineIds)
    {
        await _ordineRepository.DeleteManyAsync(ordineIds);
    }

    [Authorize(LFGPermissions.Ordini.Delete)]
    public virtual async Task DeleteAllAsync(GetOrdiniInput input)
    {
        await _ordineRepository.DeleteAllAsync(input.FilterText, input.DataOrdineMin, input.DataOrdineMax, input.Stato, input.ImportoTotaleMin, input.ImportoTotaleMax, input.IndSpedizione, input.MetodoPagamento, input.ClienteId, input.ScontoId);
    }

    public virtual async Task<LFG.Shared.DownloadTokenResultDto> GetDownloadTokenAsync()
    {
        var token = Guid.NewGuid().ToString("N");
        await _downloadTokenCache.SetAsync(token, new OrdineDownloadTokenCacheItem { Token = token }, new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30) });
        return new LFG.Shared.DownloadTokenResultDto
        {
            Token = token
        };
    }
}