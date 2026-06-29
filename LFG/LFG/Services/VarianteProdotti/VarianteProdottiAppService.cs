using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using LFG.Permissions;
using LFG.VarianteProdotti;

namespace LFG.VarianteProdotti;

[Authorize(LFGPermissions.VarianteProdotti.Default)]
public abstract class VarianteProdottiAppServiceBase : ApplicationService
{
    protected IVarianteProdottoRepository _varianteProdottoRepository;
    protected VarianteProdottoManager _varianteProdottoManager;

    public VarianteProdottiAppServiceBase(IVarianteProdottoRepository varianteProdottoRepository, VarianteProdottoManager varianteProdottoManager)
    {
        _varianteProdottoRepository = varianteProdottoRepository;
        _varianteProdottoManager = varianteProdottoManager;
    }

    public virtual async Task<PagedResultDto<VarianteProdottoDto>> GetListByProdottoIdAsync(GetVarianteProdottoListInput input)
    {
        var varianteProdotti = await _varianteProdottoRepository.GetListByProdottoIdAsync(input.ProdottoId, input.Sorting, input.MaxResultCount, input.SkipCount);
        return new PagedResultDto<VarianteProdottoDto>
        {
            TotalCount = await _varianteProdottoRepository.GetCountByProdottoIdAsync(input.ProdottoId),
            Items = ObjectMapper.Map<List<VarianteProdotto>, List<VarianteProdottoDto>>(varianteProdotti)
        };
    }

    public virtual async Task<PagedResultDto<VarianteProdottoDto>> GetListAsync(GetVarianteProdottiInput input)
    {
        var totalCount = await _varianteProdottoRepository.GetCountAsync(input.FilterText, input.Taglia, input.Colore, input.Materiale, input.UrlImmagine, input.QtaMagazzinoMin, input.QtaMagazzinoMax);
        var items = await _varianteProdottoRepository.GetListAsync(input.FilterText, input.Taglia, input.Colore, input.Materiale, input.UrlImmagine, input.QtaMagazzinoMin, input.QtaMagazzinoMax, input.Sorting, input.MaxResultCount, input.SkipCount);
        return new PagedResultDto<VarianteProdottoDto>
        {
            TotalCount = totalCount,
            Items = ObjectMapper.Map<List<VarianteProdotto>, List<VarianteProdottoDto>>(items)
        };
    }

    public virtual async Task<VarianteProdottoDto> GetAsync(Guid id)
    {
        return ObjectMapper.Map<VarianteProdotto, VarianteProdottoDto>(await _varianteProdottoRepository.GetAsync(id));
    }

    [Authorize(LFGPermissions.VarianteProdotti.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _varianteProdottoRepository.DeleteAsync(id);
    }

    [Authorize(LFGPermissions.VarianteProdotti.Create)]
    public virtual async Task<VarianteProdottoDto> CreateAsync(VarianteProdottoCreateDto input)
    {
        var varianteProdotto = await _varianteProdottoManager.CreateAsync(input.ProdottoId, input.QtaMagazzino, input.Taglia, input.Colore, input.Materiale, input.UrlImmagine);
        return ObjectMapper.Map<VarianteProdotto, VarianteProdottoDto>(varianteProdotto);
    }

    [Authorize(LFGPermissions.VarianteProdotti.Edit)]
    public virtual async Task<VarianteProdottoDto> UpdateAsync(Guid id, VarianteProdottoUpdateDto input)
    {
        var varianteProdotto = await _varianteProdottoManager.UpdateAsync(id, input.ProdottoId, input.QtaMagazzino, input.Taglia, input.Colore, input.Materiale, input.UrlImmagine);
        return ObjectMapper.Map<VarianteProdotto, VarianteProdottoDto>(varianteProdotto);
    }
}