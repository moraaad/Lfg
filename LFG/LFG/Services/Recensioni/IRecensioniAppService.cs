using LFG.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;
using LFG.Shared;

namespace LFG.Recensioni;

public partial interface IRecensioniAppService : IApplicationService
{
    Task<PagedResultDto<RecensioneWithNavigationPropertiesDto>> GetListAsync(GetRecensioniInput input);
    Task<RecensioneWithNavigationPropertiesDto> GetWithNavigationPropertiesAsync(Guid id);
    Task<RecensioneDto> GetAsync(Guid id);
    Task<PagedResultDto<LookupDto<Guid>>> GetClienteLookupAsync(LookupRequestDto input);
    Task<PagedResultDto<LookupDto<Guid>>> GetProdottoLookupAsync(LookupRequestDto input);
    Task DeleteAsync(Guid id);
    Task<RecensioneDto> CreateAsync(RecensioneCreateDto input);
    Task<RecensioneDto> UpdateAsync(Guid id, RecensioneUpdateDto input);
    Task<IRemoteStreamContent> GetListAsExcelFileAsync(RecensioneExcelDownloadDto input);
    Task DeleteByIdsAsync(List<Guid> recensioneIds);
    Task DeleteAllAsync(GetRecensioniInput input);
    Task<LFG.Shared.DownloadTokenResultDto> GetDownloadTokenAsync();
}