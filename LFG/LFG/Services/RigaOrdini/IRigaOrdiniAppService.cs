using LFG.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;
using LFG.Shared;

namespace LFG.RigaOrdini;

public partial interface IRigaOrdiniAppService : IApplicationService
{
    Task<PagedResultDto<RigaOrdineWithNavigationPropertiesDto>> GetListAsync(GetRigaOrdiniInput input);
    Task<RigaOrdineWithNavigationPropertiesDto> GetWithNavigationPropertiesAsync(Guid id);
    Task<RigaOrdineDto> GetAsync(Guid id);
    Task<PagedResultDto<LookupDto<Guid>>> GetOrdineLookupAsync(LookupRequestDto input);
    Task<PagedResultDto<LookupDto<Guid>>> GetVarianteProdottoLookupAsync(LookupRequestDto input);
    Task DeleteAsync(Guid id);
    Task<RigaOrdineDto> CreateAsync(RigaOrdineCreateDto input);
    Task<RigaOrdineDto> UpdateAsync(Guid id, RigaOrdineUpdateDto input);
    Task<IRemoteStreamContent> GetListAsExcelFileAsync(RigaOrdineExcelDownloadDto input);
    Task DeleteByIdsAsync(List<Guid> rigaordineIds);
    Task DeleteAllAsync(GetRigaOrdiniInput input);
    Task<LFG.Shared.DownloadTokenResultDto> GetDownloadTokenAsync();
}