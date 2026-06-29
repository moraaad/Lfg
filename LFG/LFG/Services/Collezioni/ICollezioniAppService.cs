using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;
using LFG.Shared;

namespace LFG.Collezioni;

public partial interface ICollezioniAppService : IApplicationService
{
    Task<PagedResultDto<CollezioneDto>> GetListAsync(GetCollezioniInput input);
    Task<CollezioneDto> GetAsync(Guid id);
    Task DeleteAsync(Guid id);
    Task<CollezioneDto> CreateAsync(CollezioneCreateDto input);
    Task<CollezioneDto> UpdateAsync(Guid id, CollezioneUpdateDto input);
    Task<IRemoteStreamContent> GetListAsExcelFileAsync(CollezioneExcelDownloadDto input);
    Task DeleteByIdsAsync(List<Guid> collezioneIds);
    Task DeleteAllAsync(GetCollezioniInput input);
    Task<LFG.Shared.DownloadTokenResultDto> GetDownloadTokenAsync();
}