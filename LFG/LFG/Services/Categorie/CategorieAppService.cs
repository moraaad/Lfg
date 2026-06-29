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
using LFG.Categorie;
using MiniExcelLibs;
using Volo.Abp.Content;
using Volo.Abp.Authorization;
using Volo.Abp.Caching;
using Microsoft.Extensions.Caching.Distributed;
using LFG.Shared;

namespace LFG.Categorie;

[Authorize(LFGPermissions.Categorie.Default)]
public abstract class CategorieAppServiceBase : ApplicationService
{
    protected IDistributedCache<CategoriaDownloadTokenCacheItem, string> _downloadTokenCache;
    protected ICategoriaRepository _categoriaRepository;
    protected CategoriaManager _categoriaManager;

    public CategorieAppServiceBase(ICategoriaRepository categoriaRepository, CategoriaManager categoriaManager, IDistributedCache<CategoriaDownloadTokenCacheItem, string> downloadTokenCache)
    {
        _downloadTokenCache = downloadTokenCache;
        _categoriaRepository = categoriaRepository;
        _categoriaManager = categoriaManager;
    }

    public virtual async Task<PagedResultDto<CategoriaDto>> GetListAsync(GetCategorieInput input)
    {
        var totalCount = await _categoriaRepository.GetCountAsync(input.FilterText, input.Nome, input.Descrizione, input.Sezione);
        var items = await _categoriaRepository.GetListAsync(input.FilterText, input.Nome, input.Descrizione, input.Sezione, input.Sorting, input.MaxResultCount, input.SkipCount);
        return new PagedResultDto<CategoriaDto>
        {
            TotalCount = totalCount,
            Items = ObjectMapper.Map<List<Categoria>, List<CategoriaDto>>(items)
        };
    }

    public virtual async Task<CategoriaDto> GetAsync(Guid id)
    {
        return ObjectMapper.Map<Categoria, CategoriaDto>(await _categoriaRepository.GetAsync(id));
    }

    [Authorize(LFGPermissions.Categorie.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _categoriaRepository.DeleteAsync(id);
    }

    [Authorize(LFGPermissions.Categorie.Create)]
    public virtual async Task<CategoriaDto> CreateAsync(CategoriaCreateDto input)
    {
        var categoria = await _categoriaManager.CreateAsync(input.Nome, input.Sezione, input.Descrizione);
        return ObjectMapper.Map<Categoria, CategoriaDto>(categoria);
    }

    [Authorize(LFGPermissions.Categorie.Edit)]
    public virtual async Task<CategoriaDto> UpdateAsync(Guid id, CategoriaUpdateDto input)
    {
        var categoria = await _categoriaManager.UpdateAsync(id, input.Nome, input.Sezione, input.Descrizione, input.ConcurrencyStamp);
        return ObjectMapper.Map<Categoria, CategoriaDto>(categoria);
    }

    [AllowAnonymous]
    public virtual async Task<IRemoteStreamContent> GetListAsExcelFileAsync(CategoriaExcelDownloadDto input)
    {
        var downloadToken = await _downloadTokenCache.GetAsync(input.DownloadToken);
        if (downloadToken == null || input.DownloadToken != downloadToken.Token)
        {
            throw new AbpAuthorizationException("Invalid download token: " + input.DownloadToken);
        }

        var items = await _categoriaRepository.GetListAsync(input.FilterText, input.Nome, input.Descrizione, input.Sezione);
        var memoryStream = new MemoryStream();
        await memoryStream.SaveAsAsync(ObjectMapper.Map<List<Categoria>, List<CategoriaExcelDto>>(items));
        memoryStream.Seek(0, SeekOrigin.Begin);
        return new RemoteStreamContent(memoryStream, "Categorie.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
    }

    [Authorize(LFGPermissions.Categorie.Delete)]
    public virtual async Task DeleteByIdsAsync(List<Guid> categoriaIds)
    {
        await _categoriaRepository.DeleteManyAsync(categoriaIds);
    }

    [Authorize(LFGPermissions.Categorie.Delete)]
    public virtual async Task DeleteAllAsync(GetCategorieInput input)
    {
        await _categoriaRepository.DeleteAllAsync(input.FilterText, input.Nome, input.Descrizione, input.Sezione);
    }

    public virtual async Task<LFG.Shared.DownloadTokenResultDto> GetDownloadTokenAsync()
    {
        var token = Guid.NewGuid().ToString("N");
        await _downloadTokenCache.SetAsync(token, new CategoriaDownloadTokenCacheItem { Token = token }, new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30) });
        return new LFG.Shared.DownloadTokenResultDto
        {
            Token = token
        };
    }
}