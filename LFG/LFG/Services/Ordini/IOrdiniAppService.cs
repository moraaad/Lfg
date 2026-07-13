using LFG.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;
using LFG.Shared;

namespace LFG.Ordini;

public partial interface IOrdiniAppService : IApplicationService
{
    Task<PagedResultDto<OrdineWithNavigationPropertiesDto>> GetListAsync(GetOrdiniInput input);
    Task<OrdineWithNavigationPropertiesDto> GetWithNavigationPropertiesAsync(Guid id);
    Task<OrdineDto> GetAsync(Guid id);
    Task<PagedResultDto<LookupDto<Guid>>> GetClienteLookupAsync(LookupRequestDto input);
    Task<PagedResultDto<LookupDto<Guid>>> GetScontoLookupAsync(LookupRequestDto input);
    Task<PagedResultDto<LookupDto<Guid>>> GetIndirizzoLookupAsync(LookupRequestDto input);
    Task DeleteAsync(Guid id);
    Task<OrdineDto> CreateAsync(OrdineCreateDto input);
    Task<OrdineDto> UpdateAsync(Guid id, OrdineUpdateDto input);
    Task<IRemoteStreamContent> GetListAsExcelFileAsync(OrdineExcelDownloadDto input);
    Task DeleteByIdsAsync(List<Guid> ordineIds);
    Task DeleteAllAsync(GetOrdiniInput input);
    Task<LFG.Shared.DownloadTokenResultDto> GetDownloadTokenAsync();
}