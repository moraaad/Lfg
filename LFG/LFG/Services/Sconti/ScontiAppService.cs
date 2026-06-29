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
using LFG.Sconti;
using MiniExcelLibs;
using Volo.Abp.Content;
using Volo.Abp.Authorization;
using Volo.Abp.Caching;
using Microsoft.Extensions.Caching.Distributed;
using LFG.Shared;

namespace LFG.Sconti;

[Authorize(LFGPermissions.Sconti.Default)]
public abstract class ScontiAppServiceBase : ApplicationService
{
    protected IDistributedCache<ScontoDownloadTokenCacheItem, string> _downloadTokenCache;
    protected IScontoRepository _scontoRepository;
    protected ScontoManager _scontoManager;

    public ScontiAppServiceBase(IScontoRepository scontoRepository, ScontoManager scontoManager, IDistributedCache<ScontoDownloadTokenCacheItem, string> downloadTokenCache)
    {
        _downloadTokenCache = downloadTokenCache;
        _scontoRepository = scontoRepository;
        _scontoManager = scontoManager;
    }

    public virtual async Task<PagedResultDto<ScontoDto>> GetListAsync(GetScontiInput input)
    {
        var totalCount = await _scontoRepository.GetCountAsync(input.FilterText, input.Codice, input.Tipo, input.ValoreMin, input.ValoreMax, input.LimiteUtilizziMin, input.LimiteUtilizziMax, input.ValidoDalMin, input.ValidoDalMax, input.ValidoAlMin, input.ValidoAlMax);
        var items = await _scontoRepository.GetListAsync(input.FilterText, input.Codice, input.Tipo, input.ValoreMin, input.ValoreMax, input.LimiteUtilizziMin, input.LimiteUtilizziMax, input.ValidoDalMin, input.ValidoDalMax, input.ValidoAlMin, input.ValidoAlMax, input.Sorting, input.MaxResultCount, input.SkipCount);
        return new PagedResultDto<ScontoDto>
        {
            TotalCount = totalCount,
            Items = ObjectMapper.Map<List<Sconto>, List<ScontoDto>>(items)
        };
    }

    public virtual async Task<ScontoDto> GetAsync(Guid id)
    {
        return ObjectMapper.Map<Sconto, ScontoDto>(await _scontoRepository.GetAsync(id));
    }

    [Authorize(LFGPermissions.Sconti.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _scontoRepository.DeleteAsync(id);
    }

    [Authorize(LFGPermissions.Sconti.Create)]
    public virtual async Task<ScontoDto> CreateAsync(ScontoCreateDto input)
    {
        var sconto = await _scontoManager.CreateAsync(input.Codice, input.Valore, input.ValidoDal, input.ValidoAl, input.Tipo, input.LimiteUtilizzi);
        return ObjectMapper.Map<Sconto, ScontoDto>(sconto);
    }

    [Authorize(LFGPermissions.Sconti.Edit)]
    public virtual async Task<ScontoDto> UpdateAsync(Guid id, ScontoUpdateDto input)
    {
        var sconto = await _scontoManager.UpdateAsync(id, input.Codice, input.Valore, input.ValidoDal, input.ValidoAl, input.Tipo, input.LimiteUtilizzi, input.ConcurrencyStamp);
        return ObjectMapper.Map<Sconto, ScontoDto>(sconto);
    }

    [AllowAnonymous]
    public virtual async Task<IRemoteStreamContent> GetListAsExcelFileAsync(ScontoExcelDownloadDto input)
    {
        var downloadToken = await _downloadTokenCache.GetAsync(input.DownloadToken);
        if (downloadToken == null || input.DownloadToken != downloadToken.Token)
        {
            throw new AbpAuthorizationException("Invalid download token: " + input.DownloadToken);
        }

        var items = await _scontoRepository.GetListAsync(input.FilterText, input.Codice, input.Tipo, input.ValoreMin, input.ValoreMax, input.LimiteUtilizziMin, input.LimiteUtilizziMax, input.ValidoDalMin, input.ValidoDalMax, input.ValidoAlMin, input.ValidoAlMax);
        var memoryStream = new MemoryStream();
        await memoryStream.SaveAsAsync(ObjectMapper.Map<List<Sconto>, List<ScontoExcelDto>>(items));
        memoryStream.Seek(0, SeekOrigin.Begin);
        return new RemoteStreamContent(memoryStream, "Sconti.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
    }

    [Authorize(LFGPermissions.Sconti.Delete)]
    public virtual async Task DeleteByIdsAsync(List<Guid> scontoIds)
    {
        await _scontoRepository.DeleteManyAsync(scontoIds);
    }

    [Authorize(LFGPermissions.Sconti.Delete)]
    public virtual async Task DeleteAllAsync(GetScontiInput input)
    {
        await _scontoRepository.DeleteAllAsync(input.FilterText, input.Codice, input.Tipo, input.ValoreMin, input.ValoreMax, input.LimiteUtilizziMin, input.LimiteUtilizziMax, input.ValidoDalMin, input.ValidoDalMax, input.ValidoAlMin, input.ValidoAlMax);
    }

    public virtual async Task<LFG.Shared.DownloadTokenResultDto> GetDownloadTokenAsync()
    {
        var token = Guid.NewGuid().ToString("N");
        await _downloadTokenCache.SetAsync(token, new ScontoDownloadTokenCacheItem { Token = token }, new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30) });
        return new LFG.Shared.DownloadTokenResultDto
        {
            Token = token
        };
    }
}