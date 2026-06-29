using LFG.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;
using LFG.Shared;

namespace LFG.ListeDesideri;

public partial interface IListeDesideriAppService : IApplicationService
{
    Task<PagedResultDto<ListaDesideriWithNavigationPropertiesDto>> GetListAsync(GetListeDesideriInput input);
    Task<ListaDesideriWithNavigationPropertiesDto> GetWithNavigationPropertiesAsync(Guid id);
    Task<ListaDesideriDto> GetAsync(Guid id);
    Task<PagedResultDto<LookupDto<Guid>>> GetClienteLookupAsync(LookupRequestDto input);
    Task DeleteAsync(Guid id);
    Task<ListaDesideriDto> CreateAsync(ListaDesideriCreateDto input);
    Task<ListaDesideriDto> UpdateAsync(Guid id, ListaDesideriUpdateDto input);
    Task<IRemoteStreamContent> GetListAsExcelFileAsync(ListaDesideriExcelDownloadDto input);
    Task DeleteByIdsAsync(List<Guid> listadesideriIds);
    Task DeleteAllAsync(GetListeDesideriInput input);
    Task<LFG.Shared.DownloadTokenResultDto> GetDownloadTokenAsync();
}