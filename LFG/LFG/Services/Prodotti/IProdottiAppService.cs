using LFG.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;
using LFG.Shared;

namespace LFG.Prodotti;

public partial interface IProdottiAppService : IApplicationService
{
    Task<PagedResultDto<ProdottoWithNavigationPropertiesDto>> GetListAsync(GetProdottiInput input);
    Task<ProdottoWithNavigationPropertiesDto> GetWithNavigationPropertiesAsync(Guid id);
    Task<ProdottoDto> GetAsync(Guid id);
    Task<PagedResultDto<LookupDto<Guid>>> GetCategoriaLookupAsync(LookupRequestDto input);
    Task<PagedResultDto<LookupDto<Guid>>> GetCollezioneLookupAsync(LookupRequestDto input);
    Task DeleteAsync(Guid id);
    Task<ProdottoDto> CreateAsync(ProdottoCreateDto input);
    Task<ProdottoDto> UpdateAsync(Guid id, ProdottoUpdateDto input);
    Task<IRemoteStreamContent> GetListAsExcelFileAsync(ProdottoExcelDownloadDto input);
    Task DeleteByIdsAsync(List<Guid> prodottoIds);
    Task DeleteAllAsync(GetProdottiInput input);
    Task<LFG.Shared.DownloadTokenResultDto> GetDownloadTokenAsync();
}