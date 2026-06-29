using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;
using LFG.Shared;

namespace LFG.Categorie;

public partial interface ICategorieAppService : IApplicationService
{
    Task<PagedResultDto<CategoriaDto>> GetListAsync(GetCategorieInput input);
    Task<CategoriaDto> GetAsync(Guid id);
    Task DeleteAsync(Guid id);
    Task<CategoriaDto> CreateAsync(CategoriaCreateDto input);
    Task<CategoriaDto> UpdateAsync(Guid id, CategoriaUpdateDto input);
    Task<IRemoteStreamContent> GetListAsExcelFileAsync(CategoriaExcelDownloadDto input);
    Task DeleteByIdsAsync(List<Guid> categoriaIds);
    Task DeleteAllAsync(GetCategorieInput input);
    Task<LFG.Shared.DownloadTokenResultDto> GetDownloadTokenAsync();
}