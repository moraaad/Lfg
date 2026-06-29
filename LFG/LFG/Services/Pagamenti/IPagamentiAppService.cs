using LFG.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;
using LFG.Shared;

namespace LFG.Pagamenti;

public partial interface IPagamentiAppService : IApplicationService
{
    Task<PagedResultDto<PagamentoWithNavigationPropertiesDto>> GetListAsync(GetPagamentiInput input);
    Task<PagamentoWithNavigationPropertiesDto> GetWithNavigationPropertiesAsync(Guid id);
    Task<PagamentoDto> GetAsync(Guid id);
    Task<PagedResultDto<LookupDto<Guid>>> GetOrdineLookupAsync(LookupRequestDto input);
    Task DeleteAsync(Guid id);
    Task<PagamentoDto> CreateAsync(PagamentoCreateDto input);
    Task<PagamentoDto> UpdateAsync(Guid id, PagamentoUpdateDto input);
    Task<IRemoteStreamContent> GetListAsExcelFileAsync(PagamentoExcelDownloadDto input);
    Task DeleteByIdsAsync(List<Guid> pagamentoIds);
    Task DeleteAllAsync(GetPagamentiInput input);
    Task<LFG.Shared.DownloadTokenResultDto> GetDownloadTokenAsync();
}