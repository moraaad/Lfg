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
using LFG.Clienti;
using MiniExcelLibs;
using Volo.Abp.Content;
using Volo.Abp.Authorization;
using Volo.Abp.Caching;
using Microsoft.Extensions.Caching.Distributed;
using LFG.Shared;

namespace LFG.Clienti;

[Authorize(LFGPermissions.Clienti.Default)]
public abstract class ClientiAppServiceBase : ApplicationService
{
    protected IDistributedCache<ClienteDownloadTokenCacheItem, string> _downloadTokenCache;
    protected IClienteRepository _clienteRepository;
    protected ClienteManager _clienteManager;

    public ClientiAppServiceBase(IClienteRepository clienteRepository, ClienteManager clienteManager, IDistributedCache<ClienteDownloadTokenCacheItem, string> downloadTokenCache)
    {
        _downloadTokenCache = downloadTokenCache;
        _clienteRepository = clienteRepository;
        _clienteManager = clienteManager;
    }

    public virtual async Task<PagedResultDto<ClienteDto>> GetListAsync(GetClientiInput input)
    {
        var totalCount = await _clienteRepository.GetCountAsync(input.FilterText, input.Nome, input.Cognome, input.Genere, input.DataNascitaMin, input.DataNascitaMax, input.Email, input.Telefono, input.Sezione, input.Nazionalita);
        var items = await _clienteRepository.GetListAsync(input.FilterText, input.Nome, input.Cognome, input.Genere, input.DataNascitaMin, input.DataNascitaMax, input.Email, input.Telefono, input.Sezione, input.Nazionalita, input.Sorting, input.MaxResultCount, input.SkipCount);
        return new PagedResultDto<ClienteDto>
        {
            TotalCount = totalCount,
            Items = ObjectMapper.Map<List<Cliente>, List<ClienteDto>>(items)
        };
    }

    public virtual async Task<ClienteDto> GetAsync(Guid id)
    {
        return ObjectMapper.Map<Cliente, ClienteDto>(await _clienteRepository.GetAsync(id));
    }

    [Authorize(LFGPermissions.Clienti.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _clienteRepository.DeleteAsync(id);
    }

    [Authorize(LFGPermissions.Clienti.Create)]
    public virtual async Task<ClienteDto> CreateAsync(ClienteCreateDto input)
    {
        var cliente = await _clienteManager.CreateAsync(input.Nome, input.Cognome, input.Genere, input.Email, input.Telefono, input.Sezione, input.Nazionalita, input.DataNascita);
        return ObjectMapper.Map<Cliente, ClienteDto>(cliente);
    }

    [Authorize(LFGPermissions.Clienti.Edit)]
    public virtual async Task<ClienteDto> UpdateAsync(Guid id, ClienteUpdateDto input)
    {
        var cliente = await _clienteManager.UpdateAsync(id, input.Nome, input.Cognome, input.Genere, input.Email, input.Telefono, input.Sezione, input.Nazionalita, input.DataNascita, input.ConcurrencyStamp);
        return ObjectMapper.Map<Cliente, ClienteDto>(cliente);
    }

    [AllowAnonymous]
    public virtual async Task<IRemoteStreamContent> GetListAsExcelFileAsync(ClienteExcelDownloadDto input)
    {
        var downloadToken = await _downloadTokenCache.GetAsync(input.DownloadToken);
        if (downloadToken == null || input.DownloadToken != downloadToken.Token)
        {
            throw new AbpAuthorizationException("Invalid download token: " + input.DownloadToken);
        }

        var items = await _clienteRepository.GetListAsync(input.FilterText, input.Nome, input.Cognome, input.Genere, input.DataNascitaMin, input.DataNascitaMax, input.Email, input.Telefono, input.Sezione, input.Nazionalita);
        var memoryStream = new MemoryStream();
        await memoryStream.SaveAsAsync(ObjectMapper.Map<List<Cliente>, List<ClienteExcelDto>>(items));
        memoryStream.Seek(0, SeekOrigin.Begin);
        return new RemoteStreamContent(memoryStream, "Clienti.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
    }

    [Authorize(LFGPermissions.Clienti.Delete)]
    public virtual async Task DeleteByIdsAsync(List<Guid> clienteIds)
    {
        await _clienteRepository.DeleteManyAsync(clienteIds);
    }

    [Authorize(LFGPermissions.Clienti.Delete)]
    public virtual async Task DeleteAllAsync(GetClientiInput input)
    {
        await _clienteRepository.DeleteAllAsync(input.FilterText, input.Nome, input.Cognome, input.Genere, input.DataNascitaMin, input.DataNascitaMax, input.Email, input.Telefono, input.Sezione, input.Nazionalita);
    }

    public virtual async Task<LFG.Shared.DownloadTokenResultDto> GetDownloadTokenAsync()
    {
        var token = Guid.NewGuid().ToString("N");
        await _downloadTokenCache.SetAsync(token, new ClienteDownloadTokenCacheItem { Token = token }, new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30) });
        return new LFG.Shared.DownloadTokenResultDto
        {
            Token = token
        };
    }
}