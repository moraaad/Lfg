using LFG.Shared;
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
using LFG.Pagamenti;
using MiniExcelLibs;
using Volo.Abp.Content;
using Volo.Abp.Authorization;
using Volo.Abp.Caching;
using Microsoft.Extensions.Caching.Distributed;
using LFG.Shared;

namespace LFG.Pagamenti;

[Authorize(LFGPermissions.Pagamenti.Default)]
public abstract class PagamentiAppServiceBase : ApplicationService
{
    protected IDistributedCache<PagamentoDownloadTokenCacheItem, string> _downloadTokenCache;
    protected IPagamentoRepository _pagamentoRepository;
    protected PagamentoManager _pagamentoManager;
    protected IRepository<LFG.Ordini.Ordine, Guid> _ordineRepository;

    public PagamentiAppServiceBase(IPagamentoRepository pagamentoRepository, PagamentoManager pagamentoManager, IDistributedCache<PagamentoDownloadTokenCacheItem, string> downloadTokenCache, IRepository<LFG.Ordini.Ordine, Guid> ordineRepository)
    {
        _downloadTokenCache = downloadTokenCache;
        _pagamentoRepository = pagamentoRepository;
        _pagamentoManager = pagamentoManager;
        _ordineRepository = ordineRepository;
    }

    public virtual async Task<PagedResultDto<PagamentoWithNavigationPropertiesDto>> GetListAsync(GetPagamentiInput input)
    {
        var totalCount = await _pagamentoRepository.GetCountAsync(input.FilterText, input.Metodo, input.Stato, input.ImportoMin, input.ImportoMax, input.DataPagamentoMin, input.DataPagamentoMax, input.IdTransazione, input.OrdineId);
        var items = await _pagamentoRepository.GetListWithNavigationPropertiesAsync(input.FilterText, input.Metodo, input.Stato, input.ImportoMin, input.ImportoMax, input.DataPagamentoMin, input.DataPagamentoMax, input.IdTransazione, input.OrdineId, input.Sorting, input.MaxResultCount, input.SkipCount);
        return new PagedResultDto<PagamentoWithNavigationPropertiesDto>
        {
            TotalCount = totalCount,
            Items = ObjectMapper.Map<List<PagamentoWithNavigationProperties>, List<PagamentoWithNavigationPropertiesDto>>(items)
        };
    }

    public virtual async Task<PagamentoWithNavigationPropertiesDto> GetWithNavigationPropertiesAsync(Guid id)
    {
        return ObjectMapper.Map<PagamentoWithNavigationProperties, PagamentoWithNavigationPropertiesDto>(await _pagamentoRepository.GetWithNavigationPropertiesAsync(id));
    }

    public virtual async Task<PagamentoDto> GetAsync(Guid id)
    {
        return ObjectMapper.Map<Pagamento, PagamentoDto>(await _pagamentoRepository.GetAsync(id));
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

    [Authorize(LFGPermissions.Pagamenti.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _pagamentoRepository.DeleteAsync(id);
    }

    [Authorize(LFGPermissions.Pagamenti.Create)]
    public virtual async Task<PagamentoDto> CreateAsync(PagamentoCreateDto input)
    {
        var pagamento = await _pagamentoManager.CreateAsync(input.OrdineId, input.Importo, input.DataPagamento, input.IdTransazione, input.Metodo, input.Stato);
        return ObjectMapper.Map<Pagamento, PagamentoDto>(pagamento);
    }

    [Authorize(LFGPermissions.Pagamenti.Edit)]
    public virtual async Task<PagamentoDto> UpdateAsync(Guid id, PagamentoUpdateDto input)
    {
        var pagamento = await _pagamentoManager.UpdateAsync(id, input.OrdineId, input.Importo, input.DataPagamento, input.IdTransazione, input.Metodo, input.Stato, input.ConcurrencyStamp);
        return ObjectMapper.Map<Pagamento, PagamentoDto>(pagamento);
    }

    [AllowAnonymous]
    public virtual async Task<IRemoteStreamContent> GetListAsExcelFileAsync(PagamentoExcelDownloadDto input)
    {
        var downloadToken = await _downloadTokenCache.GetAsync(input.DownloadToken);
        if (downloadToken == null || input.DownloadToken != downloadToken.Token)
        {
            throw new AbpAuthorizationException("Invalid download token: " + input.DownloadToken);
        }

        var pagamenti = await _pagamentoRepository.GetListWithNavigationPropertiesAsync(input.FilterText, input.Metodo, input.Stato, input.ImportoMin, input.ImportoMax, input.DataPagamentoMin, input.DataPagamentoMax, input.IdTransazione, input.OrdineId);
        var items = pagamenti.Select(item => new { Metodo = item.Pagamento.Metodo, Stato = item.Pagamento.Stato, Importo = item.Pagamento.Importo, DataPagamento = item.Pagamento.DataPagamento, IdTransazione = item.Pagamento.IdTransazione, Ordine = item.Ordine?.IndSpedizione, });
        var memoryStream = new MemoryStream();
        await memoryStream.SaveAsAsync(items);
        memoryStream.Seek(0, SeekOrigin.Begin);
        return new RemoteStreamContent(memoryStream, "Pagamenti.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
    }

    [Authorize(LFGPermissions.Pagamenti.Delete)]
    public virtual async Task DeleteByIdsAsync(List<Guid> pagamentoIds)
    {
        await _pagamentoRepository.DeleteManyAsync(pagamentoIds);
    }

    [Authorize(LFGPermissions.Pagamenti.Delete)]
    public virtual async Task DeleteAllAsync(GetPagamentiInput input)
    {
        await _pagamentoRepository.DeleteAllAsync(input.FilterText, input.Metodo, input.Stato, input.ImportoMin, input.ImportoMax, input.DataPagamentoMin, input.DataPagamentoMax, input.IdTransazione, input.OrdineId);
    }

    public virtual async Task<LFG.Shared.DownloadTokenResultDto> GetDownloadTokenAsync()
    {
        var token = Guid.NewGuid().ToString("N");
        await _downloadTokenCache.SetAsync(token, new PagamentoDownloadTokenCacheItem { Token = token }, new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30) });
        return new LFG.Shared.DownloadTokenResultDto
        {
            Token = token
        };
    }
}