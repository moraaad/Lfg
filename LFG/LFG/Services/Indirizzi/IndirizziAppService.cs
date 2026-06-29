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
using LFG.Indirizzi;
using MiniExcelLibs;
using Volo.Abp.Content;
using Volo.Abp.Authorization;
using Volo.Abp.Caching;
using Microsoft.Extensions.Caching.Distributed;
using LFG.Shared;

namespace LFG.Indirizzi;

[Authorize(LFGPermissions.Indirizzi.Default)]
public abstract class IndirizziAppServiceBase : ApplicationService
{
    protected IDistributedCache<IndirizzoDownloadTokenCacheItem, string> _downloadTokenCache;
    protected IIndirizzoRepository _indirizzoRepository;
    protected IndirizzoManager _indirizzoManager;
    protected IRepository<LFG.Clienti.Cliente, Guid> _clienteRepository;

    public IndirizziAppServiceBase(IIndirizzoRepository indirizzoRepository, IndirizzoManager indirizzoManager, IDistributedCache<IndirizzoDownloadTokenCacheItem, string> downloadTokenCache, IRepository<LFG.Clienti.Cliente, Guid> clienteRepository)
    {
        _downloadTokenCache = downloadTokenCache;
        _indirizzoRepository = indirizzoRepository;
        _indirizzoManager = indirizzoManager;
        _clienteRepository = clienteRepository;
    }

    public virtual async Task<PagedResultDto<IndirizzoWithNavigationPropertiesDto>> GetListAsync(GetIndirizziInput input)
    {
        var totalCount = await _indirizzoRepository.GetCountAsync(input.FilterText, input.Paese, input.Citta, input.Provincia, input.Via, input.Cap, input.ClienteId);
        var items = await _indirizzoRepository.GetListWithNavigationPropertiesAsync(input.FilterText, input.Paese, input.Citta, input.Provincia, input.Via, input.Cap, input.ClienteId, input.Sorting, input.MaxResultCount, input.SkipCount);
        return new PagedResultDto<IndirizzoWithNavigationPropertiesDto>
        {
            TotalCount = totalCount,
            Items = ObjectMapper.Map<List<IndirizzoWithNavigationProperties>, List<IndirizzoWithNavigationPropertiesDto>>(items)
        };
    }

    public virtual async Task<IndirizzoWithNavigationPropertiesDto> GetWithNavigationPropertiesAsync(Guid id)
    {
        return ObjectMapper.Map<IndirizzoWithNavigationProperties, IndirizzoWithNavigationPropertiesDto>(await _indirizzoRepository.GetWithNavigationPropertiesAsync(id));
    }

    public virtual async Task<IndirizzoDto> GetAsync(Guid id)
    {
        return ObjectMapper.Map<Indirizzo, IndirizzoDto>(await _indirizzoRepository.GetAsync(id));
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

    [Authorize(LFGPermissions.Indirizzi.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _indirizzoRepository.DeleteAsync(id);
    }

    [Authorize(LFGPermissions.Indirizzi.Create)]
    public virtual async Task<IndirizzoDto> CreateAsync(IndirizzoCreateDto input)
    {
        var indirizzo = await _indirizzoManager.CreateAsync(input.ClienteId, input.Via, input.Cap, input.Paese, input.Citta, input.Provincia);
        return ObjectMapper.Map<Indirizzo, IndirizzoDto>(indirizzo);
    }

    [Authorize(LFGPermissions.Indirizzi.Edit)]
    public virtual async Task<IndirizzoDto> UpdateAsync(Guid id, IndirizzoUpdateDto input)
    {
        var indirizzo = await _indirizzoManager.UpdateAsync(id, input.ClienteId, input.Via, input.Cap, input.Paese, input.Citta, input.Provincia, input.ConcurrencyStamp);
        return ObjectMapper.Map<Indirizzo, IndirizzoDto>(indirizzo);
    }

    [AllowAnonymous]
    public virtual async Task<IRemoteStreamContent> GetListAsExcelFileAsync(IndirizzoExcelDownloadDto input)
    {
        var downloadToken = await _downloadTokenCache.GetAsync(input.DownloadToken);
        if (downloadToken == null || input.DownloadToken != downloadToken.Token)
        {
            throw new AbpAuthorizationException("Invalid download token: " + input.DownloadToken);
        }

        var indirizzi = await _indirizzoRepository.GetListWithNavigationPropertiesAsync(input.FilterText, input.Paese, input.Citta, input.Provincia, input.Via, input.Cap, input.ClienteId);
        var items = indirizzi.Select(item => new { Paese = item.Indirizzo.Paese, Citta = item.Indirizzo.Citta, Provincia = item.Indirizzo.Provincia, Via = item.Indirizzo.Via, Cap = item.Indirizzo.Cap, Cliente = item.Cliente?.Email, });
        var memoryStream = new MemoryStream();
        await memoryStream.SaveAsAsync(items);
        memoryStream.Seek(0, SeekOrigin.Begin);
        return new RemoteStreamContent(memoryStream, "Indirizzi.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
    }

    [Authorize(LFGPermissions.Indirizzi.Delete)]
    public virtual async Task DeleteByIdsAsync(List<Guid> indirizzoIds)
    {
        await _indirizzoRepository.DeleteManyAsync(indirizzoIds);
    }

    [Authorize(LFGPermissions.Indirizzi.Delete)]
    public virtual async Task DeleteAllAsync(GetIndirizziInput input)
    {
        await _indirizzoRepository.DeleteAllAsync(input.FilterText, input.Paese, input.Citta, input.Provincia, input.Via, input.Cap, input.ClienteId);
    }

    public virtual async Task<LFG.Shared.DownloadTokenResultDto> GetDownloadTokenAsync()
    {
        var token = Guid.NewGuid().ToString("N");
        await _downloadTokenCache.SetAsync(token, new IndirizzoDownloadTokenCacheItem { Token = token }, new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30) });
        return new LFG.Shared.DownloadTokenResultDto
        {
            Token = token
        };
    }
}