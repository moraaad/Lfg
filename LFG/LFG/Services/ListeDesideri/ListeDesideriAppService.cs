using LFG.Shared;
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
using LFG.ListeDesideri;
using MiniExcelLibs;
using Volo.Abp.Content;
using Volo.Abp.Authorization;
using Volo.Abp.Caching;
using Microsoft.Extensions.Caching.Distributed;
using LFG.Shared;

namespace LFG.ListeDesideri;

[Authorize(LFGPermissions.ListeDesideri.Default)]
public abstract class ListeDesideriAppServiceBase : ApplicationService
{
    protected IDistributedCache<ListaDesideriDownloadTokenCacheItem, string> _downloadTokenCache;
    protected IListaDesideriRepository _listaDesideriRepository;
    protected ListaDesideriManager _listaDesideriManager;
    protected IRepository<LFG.Clienti.Cliente, Guid> _clienteRepository;

    public ListeDesideriAppServiceBase(IListaDesideriRepository listaDesideriRepository, ListaDesideriManager listaDesideriManager, IDistributedCache<ListaDesideriDownloadTokenCacheItem, string> downloadTokenCache, IRepository<LFG.Clienti.Cliente, Guid> clienteRepository)
    {
        _downloadTokenCache = downloadTokenCache;
        _listaDesideriRepository = listaDesideriRepository;
        _listaDesideriManager = listaDesideriManager;
        _clienteRepository = clienteRepository;
    }

    public virtual async Task<PagedResultDto<ListaDesideriWithNavigationPropertiesDto>> GetListAsync(GetListeDesideriInput input)
    {
        var totalCount = await _listaDesideriRepository.GetCountAsync(input.FilterText, input.DataCreazioneMin, input.DataCreazioneMax, input.NomeLista, input.ClienteId);
        var items = await _listaDesideriRepository.GetListWithNavigationPropertiesAsync(input.FilterText, input.DataCreazioneMin, input.DataCreazioneMax, input.NomeLista, input.ClienteId, input.Sorting, input.MaxResultCount, input.SkipCount);
        return new PagedResultDto<ListaDesideriWithNavigationPropertiesDto>
        {
            TotalCount = totalCount,
            Items = ObjectMapper.Map<List<ListaDesideriWithNavigationProperties>, List<ListaDesideriWithNavigationPropertiesDto>>(items)
        };
    }

    public virtual async Task<ListaDesideriWithNavigationPropertiesDto> GetWithNavigationPropertiesAsync(Guid id)
    {
        return ObjectMapper.Map<ListaDesideriWithNavigationProperties, ListaDesideriWithNavigationPropertiesDto>(await _listaDesideriRepository.GetWithNavigationPropertiesAsync(id));
    }

    public virtual async Task<ListaDesideriDto> GetAsync(Guid id)
    {
        return ObjectMapper.Map<ListaDesideri, ListaDesideriDto>(await _listaDesideriRepository.GetAsync(id));
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

    [Authorize(LFGPermissions.ListeDesideri.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _listaDesideriRepository.DeleteAsync(id);
    }

    [Authorize(LFGPermissions.ListeDesideri.Create)]
    public virtual async Task<ListaDesideriDto> CreateAsync(ListaDesideriCreateDto input)
    {
        var listaDesideri = await _listaDesideriManager.CreateAsync(input.ClienteId, input.DataCreazione, input.NomeLista);
        return ObjectMapper.Map<ListaDesideri, ListaDesideriDto>(listaDesideri);
    }

    [Authorize(LFGPermissions.ListeDesideri.Edit)]
    public virtual async Task<ListaDesideriDto> UpdateAsync(Guid id, ListaDesideriUpdateDto input)
    {
        var listaDesideri = await _listaDesideriManager.UpdateAsync(id, input.ClienteId, input.DataCreazione, input.NomeLista, input.ConcurrencyStamp);
        return ObjectMapper.Map<ListaDesideri, ListaDesideriDto>(listaDesideri);
    }

    [AllowAnonymous]
    public virtual async Task<IRemoteStreamContent> GetListAsExcelFileAsync(ListaDesideriExcelDownloadDto input)
    {
        var downloadToken = await _downloadTokenCache.GetAsync(input.DownloadToken);
        if (downloadToken == null || input.DownloadToken != downloadToken.Token)
        {
            throw new AbpAuthorizationException("Invalid download token: " + input.DownloadToken);
        }

        var listeDesideri = await _listaDesideriRepository.GetListWithNavigationPropertiesAsync(input.FilterText, input.DataCreazioneMin, input.DataCreazioneMax, input.NomeLista, input.ClienteId);
        var items = listeDesideri.Select(item => new { DataCreazione = item.ListaDesideri.DataCreazione, NomeLista = item.ListaDesideri.NomeLista, Cliente = item.Cliente?.Email, });
        var memoryStream = new MemoryStream();
        await memoryStream.SaveAsAsync(items);
        memoryStream.Seek(0, SeekOrigin.Begin);
        return new RemoteStreamContent(memoryStream, "ListeDesideri.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
    }

    [Authorize(LFGPermissions.ListeDesideri.Delete)]
    public virtual async Task DeleteByIdsAsync(List<Guid> listadesideriIds)
    {
        await _listaDesideriRepository.DeleteManyAsync(listadesideriIds);
    }

    [Authorize(LFGPermissions.ListeDesideri.Delete)]
    public virtual async Task DeleteAllAsync(GetListeDesideriInput input)
    {
        await _listaDesideriRepository.DeleteAllAsync(input.FilterText, input.DataCreazioneMin, input.DataCreazioneMax, input.NomeLista, input.ClienteId);
    }

    public virtual async Task<LFG.Shared.DownloadTokenResultDto> GetDownloadTokenAsync()
    {
        var token = Guid.NewGuid().ToString("N");
        await _downloadTokenCache.SetAsync(token, new ListaDesideriDownloadTokenCacheItem { Token = token }, new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30) });
        return new LFG.Shared.DownloadTokenResultDto
        {
            Token = token
        };
    }
}