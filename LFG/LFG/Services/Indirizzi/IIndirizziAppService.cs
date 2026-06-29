using LFG.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;
using LFG.Shared;

namespace LFG.Indirizzi;

public partial interface IIndirizziAppService : IApplicationService
{
    Task<PagedResultDto<IndirizzoWithNavigationPropertiesDto>> GetListAsync(GetIndirizziInput input);
    Task<IndirizzoWithNavigationPropertiesDto> GetWithNavigationPropertiesAsync(Guid id);
    Task<IndirizzoDto> GetAsync(Guid id);
    Task<PagedResultDto<LookupDto<Guid>>> GetClienteLookupAsync(LookupRequestDto input);
    Task DeleteAsync(Guid id);
    Task<IndirizzoDto> CreateAsync(IndirizzoCreateDto input);
    Task<IndirizzoDto> UpdateAsync(Guid id, IndirizzoUpdateDto input);
    Task<IRemoteStreamContent> GetListAsExcelFileAsync(IndirizzoExcelDownloadDto input);
    Task DeleteByIdsAsync(List<Guid> indirizzoIds);
    Task DeleteAllAsync(GetIndirizziInput input);
    Task<LFG.Shared.DownloadTokenResultDto> GetDownloadTokenAsync();
}