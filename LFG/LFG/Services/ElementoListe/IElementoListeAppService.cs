using LFG.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;
using LFG.Shared;

namespace LFG.ElementoListe;

public partial interface IElementoListeAppService : IApplicationService
{
    Task<PagedResultDto<ElementoListaWithNavigationPropertiesDto>> GetListAsync(GetElementoListeInput input);
    Task<ElementoListaWithNavigationPropertiesDto> GetWithNavigationPropertiesAsync(Guid id);
    Task<ElementoListaDto> GetAsync(Guid id);
    Task<PagedResultDto<LookupDto<Guid>>> GetVarianteProdottoLookupAsync(LookupRequestDto input);
    Task<PagedResultDto<LookupDto<Guid>>> GetListaDesideriLookupAsync(LookupRequestDto input);
    Task DeleteAsync(Guid id);
    Task<ElementoListaDto> CreateAsync(ElementoListaCreateDto input);
    Task<ElementoListaDto> UpdateAsync(Guid id, ElementoListaUpdateDto input);
    Task<IRemoteStreamContent> GetListAsExcelFileAsync(ElementoListaExcelDownloadDto input);
    Task DeleteByIdsAsync(List<Guid> elementolistaIds);
    Task DeleteAllAsync(GetElementoListeInput input);
    Task<LFG.Shared.DownloadTokenResultDto> GetDownloadTokenAsync();
}