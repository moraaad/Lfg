using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;
using LFG.Shared;

namespace LFG.Clienti;

public partial interface IClientiAppService : IApplicationService
{
    Task<PagedResultDto<ClienteDto>> GetListAsync(GetClientiInput input);
    Task<ClienteDto> GetAsync(Guid id);
    Task DeleteAsync(Guid id);
    Task<ClienteDto> CreateAsync(ClienteCreateDto input);
    Task<ClienteDto> UpdateAsync(Guid id, ClienteUpdateDto input);
    Task<IRemoteStreamContent> GetListAsExcelFileAsync(ClienteExcelDownloadDto input);
    Task DeleteByIdsAsync(List<Guid> clienteIds);
    Task DeleteAllAsync(GetClientiInput input);
    Task<LFG.Shared.DownloadTokenResultDto> GetDownloadTokenAsync();
    Task<ClienteDto?> GetClienteCorrenteAsync();
    Task<string?> GetSezioneCorrenteAsync();
    Task VerificaAccessoSezioneAsync(string sezioneProdotto);
}