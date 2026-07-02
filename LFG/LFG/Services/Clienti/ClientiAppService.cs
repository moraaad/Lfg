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
using Volo.Abp.Users;

namespace LFG.Clienti;

[Authorize(LFGPermissions.Clienti.Default)]
public abstract class ClientiAppServiceBase : ApplicationService
{
    protected IDistributedCache<ClienteDownloadTokenCacheItem, string> _downloadTokenCache;
    protected IClienteRepository _clienteRepository;
    protected ClienteManager _clienteManager;
    private readonly ICurrentUser _currentUser;
   
    public ClientiAppServiceBase(IClienteRepository clienteRepository, ClienteManager clienteManager, IDistributedCache<ClienteDownloadTokenCacheItem, string> downloadTokenCache, ICurrentUser currentUser )
    {
        _downloadTokenCache = downloadTokenCache;
        _clienteRepository = clienteRepository;
        _clienteManager = clienteManager;
        _currentUser = currentUser;
    }

    public virtual async Task<PagedResultDto<ClienteDto>> GetListAsync(GetClientiInput input)
    {
        var totalCount = await _clienteRepository.GetCountAsync(input.FilterText, input.Nome, input.Cognome, input.Genere, input.DataNascitaMin, input.DataNascitaMax, input.Email, input.Telefono, input.Sezione, input.Nazionalita, input.UserId);
        var items = await _clienteRepository.GetListAsync(input.FilterText, input.Nome, input.Cognome, input.Genere, input.DataNascitaMin, input.DataNascitaMax, input.Email, input.Telefono, input.Sezione, input.Nazionalita, input.UserId, input.Sorting, input.MaxResultCount, input.SkipCount);
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
        var cliente = await _clienteManager.CreateAsync(input.Nome, input.Cognome, input.Genere, input.Email, input.Telefono, input.Sezione, input.Nazionalita, input.DataNascita, input.UserId);
        return ObjectMapper.Map<Cliente, ClienteDto>(cliente);
    }

    [Authorize(LFGPermissions.Clienti.Edit)]
    public virtual async Task<ClienteDto> UpdateAsync(Guid id, ClienteUpdateDto input)
    {
        var cliente = await _clienteManager.UpdateAsync(id, input.Nome, input.Cognome, input.Genere, input.Email, input.Telefono, input.Sezione, input.Nazionalita, input.DataNascita, input.UserId, input.ConcurrencyStamp);
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

        var items = await _clienteRepository.GetListAsync(input.FilterText, input.Nome, input.Cognome, input.Genere, input.DataNascitaMin, input.DataNascitaMax, input.Email, input.Telefono, input.Sezione, input.Nazionalita, input.UserId);
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
        await _clienteRepository.DeleteAllAsync(input.FilterText, input.Nome, input.Cognome, input.Genere, input.DataNascitaMin, input.DataNascitaMax, input.Email, input.Telefono, input.Sezione, input.Nazionalita, input.UserId);
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

    [AllowAnonymous]
    public async Task<ClienteDto?> GetClienteCorrenteAsync()
    {
        var userId = _currentUser.Id;
        if (userId == null)
            return null; // non loggato

        var cliente = await _clienteRepository
            .FirstOrDefaultAsync(c => c.UserId == userId);

        return cliente == null
            ? null  // loggato ma senza Cliente (es. admin)
            : ObjectMapper.Map<Cliente, ClienteDto>(cliente);
    }

    [AllowAnonymous]
    public async Task<string?> GetSezioneCorrenteAsync()
    {
        var userId = _currentUser.Id;
        if (userId == null)
            return null; // non loggato

        var cliente = await _clienteRepository
            .FirstOrDefaultAsync(c => c.UserId == userId);

        return cliente?.Sezione; // null se loggato ma senza Cliente
    }

    [AllowAnonymous]
    public async Task VerificaAccessoSezioneAsync(string sezioneProdotto)
    {
        var sezioneUtente = await GetSezioneCorrenteAsync();

        // null = non loggato oppure loggato senza Cliente: in entrambi i casi, vietato
        if (sezioneUtente == null)
            throw new AbpAuthorizationException(
                "Devi effettuare l'accesso per compiere questa azione.");

        if (!string.Equals(sezioneUtente, sezioneProdotto, StringComparison.OrdinalIgnoreCase))
            throw new BusinessException("LFG:CrossSezione")
                .WithData("SezioneUtente", sezioneUtente)
                .WithData("SezioneProdotto", sezioneProdotto);
    }
}