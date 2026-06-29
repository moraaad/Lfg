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
using LFG.Collezioni;
using MiniExcelLibs;
using Volo.Abp.Content;
using Volo.Abp.Authorization;
using Volo.Abp.Caching;
using Microsoft.Extensions.Caching.Distributed;
using LFG.Shared;

namespace LFG.Collezioni;

[Authorize(LFGPermissions.Collezioni.Default)]
public abstract class CollezioniAppServiceBase : ApplicationService
{
    protected IDistributedCache<CollezioneDownloadTokenCacheItem, string> _downloadTokenCache;
    protected ICollezioneRepository _collezioneRepository;
    protected CollezioneManager _collezioneManager;

    public CollezioniAppServiceBase(ICollezioneRepository collezioneRepository, CollezioneManager collezioneManager, IDistributedCache<CollezioneDownloadTokenCacheItem, string> downloadTokenCache)
    {
        _downloadTokenCache = downloadTokenCache;
        _collezioneRepository = collezioneRepository;
        _collezioneManager = collezioneManager;
    }

    public virtual async Task<PagedResultDto<CollezioneDto>> GetListAsync(GetCollezioniInput input)
    {
        var totalCount = await _collezioneRepository.GetCountAsync(input.FilterText, input.Nome, input.Stagione, input.AnnoMin, input.AnnoMax, input.Sezione);
        var items = await _collezioneRepository.GetListAsync(input.FilterText, input.Nome, input.Stagione, input.AnnoMin, input.AnnoMax, input.Sezione, input.Sorting, input.MaxResultCount, input.SkipCount);
        return new PagedResultDto<CollezioneDto>
        {
            TotalCount = totalCount,
            Items = ObjectMapper.Map<List<Collezione>, List<CollezioneDto>>(items)
        };
    }

    public virtual async Task<CollezioneDto> GetAsync(Guid id)
    {
        return ObjectMapper.Map<Collezione, CollezioneDto>(await _collezioneRepository.GetAsync(id));
    }

    [Authorize(LFGPermissions.Collezioni.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _collezioneRepository.DeleteAsync(id);
    }

    [Authorize(LFGPermissions.Collezioni.Create)]
    public virtual async Task<CollezioneDto> CreateAsync(CollezioneCreateDto input)
    {
        var collezione = await _collezioneManager.CreateAsync(input.Nome, input.Stagione, input.Anno, input.Sezione);
        return ObjectMapper.Map<Collezione, CollezioneDto>(collezione);
    }

    [Authorize(LFGPermissions.Collezioni.Edit)]
    public virtual async Task<CollezioneDto> UpdateAsync(Guid id, CollezioneUpdateDto input)
    {
        var collezione = await _collezioneManager.UpdateAsync(id, input.Nome, input.Stagione, input.Anno, input.Sezione, input.ConcurrencyStamp);
        return ObjectMapper.Map<Collezione, CollezioneDto>(collezione);
    }

    [AllowAnonymous]
    public virtual async Task<IRemoteStreamContent> GetListAsExcelFileAsync(CollezioneExcelDownloadDto input)
    {
        var downloadToken = await _downloadTokenCache.GetAsync(input.DownloadToken);
        if (downloadToken == null || input.DownloadToken != downloadToken.Token)
        {
            throw new AbpAuthorizationException("Invalid download token: " + input.DownloadToken);
        }

        var items = await _collezioneRepository.GetListAsync(input.FilterText, input.Nome, input.Stagione, input.AnnoMin, input.AnnoMax, input.Sezione);
        var memoryStream = new MemoryStream();
        await memoryStream.SaveAsAsync(ObjectMapper.Map<List<Collezione>, List<CollezioneExcelDto>>(items));
        memoryStream.Seek(0, SeekOrigin.Begin);
        return new RemoteStreamContent(memoryStream, "Collezioni.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
    }

    [Authorize(LFGPermissions.Collezioni.Delete)]
    public virtual async Task DeleteByIdsAsync(List<Guid> collezioneIds)
    {
        await _collezioneRepository.DeleteManyAsync(collezioneIds);
    }

    [Authorize(LFGPermissions.Collezioni.Delete)]
    public virtual async Task DeleteAllAsync(GetCollezioniInput input)
    {
        await _collezioneRepository.DeleteAllAsync(input.FilterText, input.Nome, input.Stagione, input.AnnoMin, input.AnnoMax, input.Sezione);
    }

    public virtual async Task<LFG.Shared.DownloadTokenResultDto> GetDownloadTokenAsync()
    {
        var token = Guid.NewGuid().ToString("N");
        await _downloadTokenCache.SetAsync(token, new CollezioneDownloadTokenCacheItem { Token = token }, new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30) });
        return new LFG.Shared.DownloadTokenResultDto
        {
            Token = token
        };
    }
}