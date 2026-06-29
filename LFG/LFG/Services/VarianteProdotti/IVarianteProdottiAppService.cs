using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LFG.VarianteProdotti;

public partial interface IVarianteProdottiAppService : IApplicationService
{
    Task<PagedResultDto<VarianteProdottoDto>> GetListByProdottoIdAsync(GetVarianteProdottoListInput input);
    Task<PagedResultDto<VarianteProdottoDto>> GetListAsync(GetVarianteProdottiInput input);
    Task<VarianteProdottoDto> GetAsync(Guid id);
    Task DeleteAsync(Guid id);
    Task<VarianteProdottoDto> CreateAsync(VarianteProdottoCreateDto input);
    Task<VarianteProdottoDto> UpdateAsync(Guid id, VarianteProdottoUpdateDto input);
}