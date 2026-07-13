using LFG.Shared;
using LFG.Collezioni;
using LFG.Categorie;
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
using LFG.Prodotti;
using MiniExcelLibs;
using Volo.Abp.Content;
using Volo.Abp.Authorization;
using Volo.Abp.Caching;
using Microsoft.Extensions.Caching.Distributed;
using LFG.Shared;

namespace LFG.Prodotti;

[Authorize(LFGPermissions.Prodotti.Default)]
public abstract class ProdottiAppServiceBase : ApplicationService
{
    protected IDistributedCache<ProdottoDownloadTokenCacheItem, string> _downloadTokenCache;
    protected IProdottoRepository _prodottoRepository;
    protected ProdottoManager _prodottoManager;
    protected IRepository<LFG.Categorie.Categoria, Guid> _categoriaRepository;
    protected IRepository<LFG.Collezioni.Collezione, Guid> _collezioneRepository;

    public ProdottiAppServiceBase(IProdottoRepository prodottoRepository, ProdottoManager prodottoManager, IDistributedCache<ProdottoDownloadTokenCacheItem, string> downloadTokenCache, IRepository<LFG.Categorie.Categoria, Guid> categoriaRepository, IRepository<LFG.Collezioni.Collezione, Guid> collezioneRepository)
    {
        _downloadTokenCache = downloadTokenCache;
        _prodottoRepository = prodottoRepository;
        _prodottoManager = prodottoManager;
        _categoriaRepository = categoriaRepository;
        _collezioneRepository = collezioneRepository;
    }

    public virtual async Task<PagedResultDto<ProdottoWithNavigationPropertiesDto>> GetListAsync(GetProdottiInput input)
    {
        var totalCount = await _prodottoRepository.GetCountAsync(input.FilterText, input.Nome, input.Descrizione, input.Prezzo, input.CodiceSku, input.Sezione, input.CategoriaId, input.CollezioneId);
        var items = await _prodottoRepository.GetListWithNavigationPropertiesAsync(input.FilterText, input.Nome, input.Descrizione, input.Prezzo, input.CodiceSku, input.Sezione, input.CategoriaId, input.CollezioneId, input.Sorting, input.MaxResultCount, input.SkipCount);
        return new PagedResultDto<ProdottoWithNavigationPropertiesDto>
        {
            TotalCount = totalCount,
            Items = ObjectMapper.Map<List<ProdottoWithNavigationProperties>, List<ProdottoWithNavigationPropertiesDto>>(items)
        };
    }

    public virtual async Task<ProdottoWithNavigationPropertiesDto> GetWithNavigationPropertiesAsync(Guid id)
    {
        return ObjectMapper.Map<ProdottoWithNavigationProperties, ProdottoWithNavigationPropertiesDto>(await _prodottoRepository.GetWithNavigationPropertiesAsync(id));
    }

    public virtual async Task<ProdottoDto> GetAsync(Guid id)
    {
        return ObjectMapper.Map<Prodotto, ProdottoDto>(await _prodottoRepository.GetAsync(id));
    }

    public virtual async Task<PagedResultDto<LookupDto<Guid>>> GetCategoriaLookupAsync(LookupRequestDto input)
    {
        var query = (await _categoriaRepository.GetQueryableAsync()).WhereIf(!string.IsNullOrWhiteSpace(input.Filter), x => x.Nome != null && x.Nome.Contains(input.Filter));
        var lookupData = await query.PageBy(input.SkipCount, input.MaxResultCount).ToDynamicListAsync<LFG.Categorie.Categoria>();
        var totalCount = query.Count();
        return new PagedResultDto<LookupDto<Guid>>
        {
            TotalCount = totalCount,
            Items = lookupData.Select(x => new LookupDto<Guid> { Id = x.Id, DisplayName = x.Nome }).ToList()
        };
    }

    public virtual async Task<PagedResultDto<LookupDto<Guid>>> GetCollezioneLookupAsync(LookupRequestDto input)
    {
        var query = (await _collezioneRepository.GetQueryableAsync()).WhereIf(!string.IsNullOrWhiteSpace(input.Filter), x => x.Nome != null && x.Nome.Contains(input.Filter));
        var lookupData = await query.PageBy(input.SkipCount, input.MaxResultCount).ToDynamicListAsync<LFG.Collezioni.Collezione>();
        var totalCount = query.Count();
        return new PagedResultDto<LookupDto<Guid>>
        {
            TotalCount = totalCount,
            Items = lookupData.Select(x => new LookupDto<Guid> { Id = x.Id, DisplayName = x.Nome }).ToList()
        };
    }

    [Authorize(LFGPermissions.Prodotti.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _prodottoRepository.DeleteAsync(id);
    }

    [Authorize(LFGPermissions.Prodotti.Create)]
    public virtual async Task<ProdottoDto> CreateAsync(ProdottoCreateDto input)
    {
        var prodotto = await _prodottoManager.CreateAsync(input.CategoriaId, input.CollezioneId, input.Nome, input.Prezzo, input.Sezione, input.Descrizione, input.CodiceSku);
        return ObjectMapper.Map<Prodotto, ProdottoDto>(prodotto);
    }

    [Authorize(LFGPermissions.Prodotti.Edit)]
    public virtual async Task<ProdottoDto> UpdateAsync(Guid id, ProdottoUpdateDto input)
    {
        var prodotto = await _prodottoManager.UpdateAsync(id, input.CategoriaId, input.CollezioneId, input.Nome, input.Prezzo, input.Sezione, input.Descrizione, input.CodiceSku, input.ConcurrencyStamp);
        return ObjectMapper.Map<Prodotto, ProdottoDto>(prodotto);
    }

    [AllowAnonymous]
    public virtual async Task<IRemoteStreamContent> GetListAsExcelFileAsync(ProdottoExcelDownloadDto input)
    {
        var downloadToken = await _downloadTokenCache.GetAsync(input.DownloadToken);
        if (downloadToken == null || input.DownloadToken != downloadToken.Token)
        {
            throw new AbpAuthorizationException("Invalid download token: " + input.DownloadToken);
        }

        var prodotti = await _prodottoRepository.GetListWithNavigationPropertiesAsync(input.FilterText, input.Nome, input.Descrizione, input.Prezzo, input.CodiceSku, input.Sezione, input.CategoriaId, input.CollezioneId);
        var items = prodotti.Select(item => new { Nome = item.Prodotto.Nome, Descrizione = item.Prodotto.Descrizione, Prezzo = item.Prodotto.Prezzo, CodiceSku = item.Prodotto.CodiceSku, Sezione = item.Prodotto.Sezione, Categoria = item.Categoria?.Nome, Collezione = item.Collezione?.Nome, });
        var memoryStream = new MemoryStream();
        await memoryStream.SaveAsAsync(items);
        memoryStream.Seek(0, SeekOrigin.Begin);
        return new RemoteStreamContent(memoryStream, "Prodotti.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
    }

    [Authorize(LFGPermissions.Prodotti.Delete)]
    public virtual async Task DeleteByIdsAsync(List<Guid> prodottoIds)
    {
        await _prodottoRepository.DeleteManyAsync(prodottoIds);
    }

    [Authorize(LFGPermissions.Prodotti.Delete)]
    public virtual async Task DeleteAllAsync(GetProdottiInput input)
    {
        await _prodottoRepository.DeleteAllAsync(input.FilterText, input.Nome, input.Descrizione, input.Prezzo, input.CodiceSku, input.Sezione, input.CategoriaId, input.CollezioneId);
    }

    public virtual async Task<LFG.Shared.DownloadTokenResultDto> GetDownloadTokenAsync()
    {
        var token = Guid.NewGuid().ToString("N");
        await _downloadTokenCache.SetAsync(token, new ProdottoDownloadTokenCacheItem { Token = token }, new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30) });
        return new LFG.Shared.DownloadTokenResultDto
        {
            Token = token
        };
    }
}