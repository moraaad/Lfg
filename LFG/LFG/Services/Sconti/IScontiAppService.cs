using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;
using LFG.Shared;

namespace LFG.Sconti;

public partial interface IScontiAppService : IApplicationService
{
    Task<PagedResultDto<ScontoDto>> GetListAsync(GetScontiInput input);
    Task<ScontoDto> GetAsync(Guid id);
    Task DeleteAsync(Guid id);
    Task<ScontoDto> CreateAsync(ScontoCreateDto input);
    Task<ScontoDto> UpdateAsync(Guid id, ScontoUpdateDto input);
    Task<IRemoteStreamContent> GetListAsExcelFileAsync(ScontoExcelDownloadDto input);
    Task DeleteByIdsAsync(List<Guid> scontoIds);
    Task DeleteAllAsync(GetScontiInput input);
    Task<LFG.Shared.DownloadTokenResultDto> GetDownloadTokenAsync();
}