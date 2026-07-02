using LFG.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;
using LFG.Shared;

namespace LFG.ImmagineVarianti;

public partial interface IImmagineVariantiAppService : IApplicationService
{
    Task<PagedResultDto<ImmagineVarianteWithNavigationPropertiesDto>> GetListAsync(GetImmagineVariantiInput input);
    Task<ImmagineVarianteWithNavigationPropertiesDto> GetWithNavigationPropertiesAsync(Guid id);
    Task<ImmagineVarianteDto> GetAsync(Guid id);
    Task<PagedResultDto<LookupDto<Guid>>> GetVarianteProdottoLookupAsync(LookupRequestDto input);
    Task DeleteAsync(Guid id);
    Task<ImmagineVarianteDto> CreateAsync(ImmagineVarianteCreateDto input);
    Task<ImmagineVarianteDto> UpdateAsync(Guid id, ImmagineVarianteUpdateDto input);
    Task<IRemoteStreamContent> GetListAsExcelFileAsync(ImmagineVarianteExcelDownloadDto input);
    Task DeleteByIdsAsync(List<Guid> immaginevarianteIds);
    Task DeleteAllAsync(GetImmagineVariantiInput input);
    Task<LFG.Shared.DownloadTokenResultDto> GetDownloadTokenAsync();
}