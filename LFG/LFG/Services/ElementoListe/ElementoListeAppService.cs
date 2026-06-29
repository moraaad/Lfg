using LFG.Shared;
using LFG.ListeDesideri;
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
using LFG.ElementoListe;
using MiniExcelLibs;
using Volo.Abp.Content;
using Volo.Abp.Authorization;
using Volo.Abp.Caching;
using Microsoft.Extensions.Caching.Distributed;
using LFG.Shared;

namespace LFG.ElementoListe;

[Authorize(LFGPermissions.ElementoListe.Default)]
public abstract class ElementoListeAppServiceBase : ApplicationService
{
    protected IDistributedCache<ElementoListaDownloadTokenCacheItem, string> _downloadTokenCache;
    protected IElementoListaRepository _elementoListaRepository;
    protected ElementoListaManager _elementoListaManager;
    protected IRepository<LFG.VarianteProdotti.VarianteProdotto, Guid> _varianteProdottoRepository;
    protected IRepository<LFG.ListeDesideri.ListaDesideri, Guid> _listaDesideriRepository;

    public ElementoListeAppServiceBase(IElementoListaRepository elementoListaRepository, ElementoListaManager elementoListaManager, IDistributedCache<ElementoListaDownloadTokenCacheItem, string> downloadTokenCache, IRepository<LFG.VarianteProdotti.VarianteProdotto, Guid> varianteProdottoRepository, IRepository<LFG.ListeDesideri.ListaDesideri, Guid> listaDesideriRepository)
    {
        _downloadTokenCache = downloadTokenCache;
        _elementoListaRepository = elementoListaRepository;
        _elementoListaManager = elementoListaManager;
        _varianteProdottoRepository = varianteProdottoRepository;
        _listaDesideriRepository = listaDesideriRepository;
    }

    public virtual async Task<PagedResultDto<ElementoListaWithNavigationPropertiesDto>> GetListAsync(GetElementoListeInput input)
    {
        var totalCount = await _elementoListaRepository.GetCountAsync(input.FilterText, input.DataAggiuntaMin, input.DataAggiuntaMax, input.VarianteProdottoId, input.ListaDesideriId);
        var items = await _elementoListaRepository.GetListWithNavigationPropertiesAsync(input.FilterText, input.DataAggiuntaMin, input.DataAggiuntaMax, input.VarianteProdottoId, input.ListaDesideriId, input.Sorting, input.MaxResultCount, input.SkipCount);
        return new PagedResultDto<ElementoListaWithNavigationPropertiesDto>
        {
            TotalCount = totalCount,
            Items = ObjectMapper.Map<List<ElementoListaWithNavigationProperties>, List<ElementoListaWithNavigationPropertiesDto>>(items)
        };
    }

    public virtual async Task<ElementoListaWithNavigationPropertiesDto> GetWithNavigationPropertiesAsync(Guid id)
    {
        return ObjectMapper.Map<ElementoListaWithNavigationProperties, ElementoListaWithNavigationPropertiesDto>(await _elementoListaRepository.GetWithNavigationPropertiesAsync(id));
    }

    public virtual async Task<ElementoListaDto> GetAsync(Guid id)
    {
        return ObjectMapper.Map<ElementoLista, ElementoListaDto>(await _elementoListaRepository.GetAsync(id));
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

    public virtual async Task<PagedResultDto<LookupDto<Guid>>> GetListaDesideriLookupAsync(LookupRequestDto input)
    {
        var query = (await _listaDesideriRepository.GetQueryableAsync()).WhereIf(!string.IsNullOrWhiteSpace(input.Filter), x => x.NomeLista != null && x.NomeLista.Contains(input.Filter));
        var lookupData = await query.PageBy(input.SkipCount, input.MaxResultCount).ToDynamicListAsync<LFG.ListeDesideri.ListaDesideri>();
        var totalCount = query.Count();
        return new PagedResultDto<LookupDto<Guid>>
        {
            TotalCount = totalCount,
            Items = lookupData.Select(x => new LookupDto<Guid> { Id = x.Id, DisplayName = x.NomeLista }).ToList()
        };
    }

    [Authorize(LFGPermissions.ElementoListe.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _elementoListaRepository.DeleteAsync(id);
    }

    [Authorize(LFGPermissions.ElementoListe.Create)]
    public virtual async Task<ElementoListaDto> CreateAsync(ElementoListaCreateDto input)
    {
        if (input.VarianteProdottoId == default)
        {
            throw new UserFriendlyException(L["The {0} field is required.", L["VarianteProdotto"]]);
        }

        var elementoLista = await _elementoListaManager.CreateAsync(input.VarianteProdottoId, input.ListaDesideriId, input.DataAggiunta);
        return ObjectMapper.Map<ElementoLista, ElementoListaDto>(elementoLista);
    }

    [Authorize(LFGPermissions.ElementoListe.Edit)]
    public virtual async Task<ElementoListaDto> UpdateAsync(Guid id, ElementoListaUpdateDto input)
    {
        if (input.VarianteProdottoId == default)
        {
            throw new UserFriendlyException(L["The {0} field is required.", L["VarianteProdotto"]]);
        }

        var elementoLista = await _elementoListaManager.UpdateAsync(id, input.VarianteProdottoId, input.ListaDesideriId, input.DataAggiunta, input.ConcurrencyStamp);
        return ObjectMapper.Map<ElementoLista, ElementoListaDto>(elementoLista);
    }

    [AllowAnonymous]
    public virtual async Task<IRemoteStreamContent> GetListAsExcelFileAsync(ElementoListaExcelDownloadDto input)
    {
        var downloadToken = await _downloadTokenCache.GetAsync(input.DownloadToken);
        if (downloadToken == null || input.DownloadToken != downloadToken.Token)
        {
            throw new AbpAuthorizationException("Invalid download token: " + input.DownloadToken);
        }

        var elementoListe = await _elementoListaRepository.GetListWithNavigationPropertiesAsync(input.FilterText, input.DataAggiuntaMin, input.DataAggiuntaMax, input.VarianteProdottoId, input.ListaDesideriId);
        var items = elementoListe.Select(item => new { DataAggiunta = item.ElementoLista.DataAggiunta, VarianteProdotto = item.VarianteProdotto?.UrlImmagine, ListaDesideri = item.ListaDesideri?.NomeLista, });
        var memoryStream = new MemoryStream();
        await memoryStream.SaveAsAsync(items);
        memoryStream.Seek(0, SeekOrigin.Begin);
        return new RemoteStreamContent(memoryStream, "ElementoListe.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
    }

    [Authorize(LFGPermissions.ElementoListe.Delete)]
    public virtual async Task DeleteByIdsAsync(List<Guid> elementolistaIds)
    {
        await _elementoListaRepository.DeleteManyAsync(elementolistaIds);
    }

    [Authorize(LFGPermissions.ElementoListe.Delete)]
    public virtual async Task DeleteAllAsync(GetElementoListeInput input)
    {
        await _elementoListaRepository.DeleteAllAsync(input.FilterText, input.DataAggiuntaMin, input.DataAggiuntaMax, input.VarianteProdottoId, input.ListaDesideriId);
    }

    public virtual async Task<LFG.Shared.DownloadTokenResultDto> GetDownloadTokenAsync()
    {
        var token = Guid.NewGuid().ToString("N");
        await _downloadTokenCache.SetAsync(token, new ElementoListaDownloadTokenCacheItem { Token = token }, new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30) });
        return new LFG.Shared.DownloadTokenResultDto
        {
            Token = token
        };
    }
}