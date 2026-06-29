using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using LFG.RigaOrdini;
using LFG.Shared;

namespace LFG.Web.Pages.RigaOrdini;

public abstract class IndexModelBase : AbpPageModel
{
    public int? QuantitaFilterMin { get; set; }

    public int? QuantitaFilterMax { get; set; }

    public decimal? PrezzoUnitarioFilterMin { get; set; }

    public decimal? PrezzoUnitarioFilterMax { get; set; }

    [SelectItems(nameof(OrdineLookupList))]
    public Guid? OrdineIdFilter { get; set; }

    public List<SelectListItem> OrdineLookupList { get; set; } = new List<SelectListItem> { new SelectListItem(string.Empty, "") };
    [SelectItems(nameof(VarianteProdottoLookupList))]
    public Guid? VarianteProdottoIdFilter { get; set; }

    public List<SelectListItem> VarianteProdottoLookupList { get; set; } = new List<SelectListItem> { new SelectListItem(string.Empty, "") };

    protected IRigaOrdiniAppService _rigaOrdiniAppService;

    public IndexModelBase(IRigaOrdiniAppService rigaOrdiniAppService)
    {
        _rigaOrdiniAppService = rigaOrdiniAppService;
    }

    public virtual async Task OnGetAsync()
    {
        OrdineLookupList.AddRange((await _rigaOrdiniAppService.GetOrdineLookupAsync(new LookupRequestDto { MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount })).Items.Select(t => new SelectListItem(t.DisplayName, t.Id.ToString())).ToList());
        VarianteProdottoLookupList.AddRange((await _rigaOrdiniAppService.GetVarianteProdottoLookupAsync(new LookupRequestDto { MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount })).Items.Select(t => new SelectListItem(t.DisplayName, t.Id.ToString())).ToList());
        await Task.CompletedTask;
    }
}