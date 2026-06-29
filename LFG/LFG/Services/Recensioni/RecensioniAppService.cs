using LFG.Shared;
using LFG.Prodotti;
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
using LFG.Recensioni;
using MiniExcelLibs;
using Volo.Abp.Content;
using Volo.Abp.Authorization;
using Volo.Abp.Caching;
using Microsoft.Extensions.Caching.Distributed;
using LFG.Shared;

namespace LFG.Recensioni;

[Authorize(LFGPermissions.Recensioni.Default)]
public abstract class RecensioniAppServiceBase : ApplicationService
{
    protected IDistributedCache<RecensioneDownloadTokenCacheItem, string> _downloadTokenCache;
    protected IRecensioneRepository _recensioneRepository;
    protected RecensioneManager _recensioneManager;
    protected IRepository<LFG.Clienti.Cliente, Guid> _clienteRepository;
    protected IRepository<LFG.Prodotti.Prodotto, Guid> _prodottoRepository;

    public RecensioniAppServiceBase(IRecensioneRepository recensioneRepository, RecensioneManager recensioneManager, IDistributedCache<RecensioneDownloadTokenCacheItem, string> downloadTokenCache, IRepository<LFG.Clienti.Cliente, Guid> clienteRepository, IRepository<LFG.Prodotti.Prodotto, Guid> prodottoRepository)
    {
        _downloadTokenCache = downloadTokenCache;
        _recensioneRepository = recensioneRepository;
        _recensioneManager = recensioneManager;
        _clienteRepository = clienteRepository;
        _prodottoRepository = prodottoRepository;
    }

    public virtual async Task<PagedResultDto<RecensioneWithNavigationPropertiesDto>> GetListAsync(GetRecensioniInput input)
    {
        var totalCount = await _recensioneRepository.GetCountAsync(input.FilterText, input.ValutazioneMin, input.ValutazioneMax, input.Commento, input.DataRecensioneMin, input.DataRecensioneMax, input.ClienteId, input.ProdottoId);
        var items = await _recensioneRepository.GetListWithNavigationPropertiesAsync(input.FilterText, input.ValutazioneMin, input.ValutazioneMax, input.Commento, input.DataRecensioneMin, input.DataRecensioneMax, input.ClienteId, input.ProdottoId, input.Sorting, input.MaxResultCount, input.SkipCount);
        return new PagedResultDto<RecensioneWithNavigationPropertiesDto>
        {
            TotalCount = totalCount,
            Items = ObjectMapper.Map<List<RecensioneWithNavigationProperties>, List<RecensioneWithNavigationPropertiesDto>>(items)
        };
    }

    public virtual async Task<RecensioneWithNavigationPropertiesDto> GetWithNavigationPropertiesAsync(Guid id)
    {
        return ObjectMapper.Map<RecensioneWithNavigationProperties, RecensioneWithNavigationPropertiesDto>(await _recensioneRepository.GetWithNavigationPropertiesAsync(id));
    }

    public virtual async Task<RecensioneDto> GetAsync(Guid id)
    {
        return ObjectMapper.Map<Recensione, RecensioneDto>(await _recensioneRepository.GetAsync(id));
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

    public virtual async Task<PagedResultDto<LookupDto<Guid>>> GetProdottoLookupAsync(LookupRequestDto input)
    {
        var query = (await _prodottoRepository.GetQueryableAsync()).WhereIf(!string.IsNullOrWhiteSpace(input.Filter), x => x.Nome != null && x.Nome.Contains(input.Filter));
        var lookupData = await query.PageBy(input.SkipCount, input.MaxResultCount).ToDynamicListAsync<LFG.Prodotti.Prodotto>();
        var totalCount = query.Count();
        return new PagedResultDto<LookupDto<Guid>>
        {
            TotalCount = totalCount,
            Items = lookupData.Select(x => new LookupDto<Guid> { Id = x.Id, DisplayName = x.Nome }).ToList()
        };
    }

    [Authorize(LFGPermissions.Recensioni.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _recensioneRepository.DeleteAsync(id);
    }

    [Authorize(LFGPermissions.Recensioni.Create)]
    public virtual async Task<RecensioneDto> CreateAsync(RecensioneCreateDto input)
    {
        var recensione = await _recensioneManager.CreateAsync(input.ClienteId, input.ProdottoId, input.Valutazione, input.DataRecensione, input.Commento);
        return ObjectMapper.Map<Recensione, RecensioneDto>(recensione);
    }

    [Authorize(LFGPermissions.Recensioni.Edit)]
    public virtual async Task<RecensioneDto> UpdateAsync(Guid id, RecensioneUpdateDto input)
    {
        var recensione = await _recensioneManager.UpdateAsync(id, input.ClienteId, input.ProdottoId, input.Valutazione, input.DataRecensione, input.Commento, input.ConcurrencyStamp);
        return ObjectMapper.Map<Recensione, RecensioneDto>(recensione);
    }

    [AllowAnonymous]
    public virtual async Task<IRemoteStreamContent> GetListAsExcelFileAsync(RecensioneExcelDownloadDto input)
    {
        var downloadToken = await _downloadTokenCache.GetAsync(input.DownloadToken);
        if (downloadToken == null || input.DownloadToken != downloadToken.Token)
        {
            throw new AbpAuthorizationException("Invalid download token: " + input.DownloadToken);
        }

        var recensioni = await _recensioneRepository.GetListWithNavigationPropertiesAsync(input.FilterText, input.ValutazioneMin, input.ValutazioneMax, input.Commento, input.DataRecensioneMin, input.DataRecensioneMax, input.ClienteId, input.ProdottoId);
        var items = recensioni.Select(item => new { Valutazione = item.Recensione.Valutazione, Commento = item.Recensione.Commento, DataRecensione = item.Recensione.DataRecensione, Cliente = item.Cliente?.Email, Prodotto = item.Prodotto?.Nome, });
        var memoryStream = new MemoryStream();
        await memoryStream.SaveAsAsync(items);
        memoryStream.Seek(0, SeekOrigin.Begin);
        return new RemoteStreamContent(memoryStream, "Recensioni.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
    }

    [Authorize(LFGPermissions.Recensioni.Delete)]
    public virtual async Task DeleteByIdsAsync(List<Guid> recensioneIds)
    {
        await _recensioneRepository.DeleteManyAsync(recensioneIds);
    }

    [Authorize(LFGPermissions.Recensioni.Delete)]
    public virtual async Task DeleteAllAsync(GetRecensioniInput input)
    {
        await _recensioneRepository.DeleteAllAsync(input.FilterText, input.ValutazioneMin, input.ValutazioneMax, input.Commento, input.DataRecensioneMin, input.DataRecensioneMax, input.ClienteId, input.ProdottoId);
    }

    public virtual async Task<LFG.Shared.DownloadTokenResultDto> GetDownloadTokenAsync()
    {
        var token = Guid.NewGuid().ToString("N");
        await _downloadTokenCache.SetAsync(token, new RecensioneDownloadTokenCacheItem { Token = token }, new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30) });
        return new LFG.Shared.DownloadTokenResultDto
        {
            Token = token
        };
    }
}