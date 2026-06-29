using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using LFG.Shared;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LFG.RigaOrdini;

namespace LFG.Web.Pages.RigaOrdini;

public abstract class CreateModalModelBase : AbpPageModel
{
    [BindProperty]
    public RigaOrdineCreateViewModel RigaOrdine { get; set; }

    public List<SelectListItem> OrdineLookupList { get; set; } = new List<SelectListItem> { new SelectListItem(" — ", "") };
    public List<SelectListItem> VarianteProdottoLookupList { get; set; } = new List<SelectListItem> { new SelectListItem(" — ", "") };

    protected IRigaOrdiniAppService _rigaOrdiniAppService;

    public CreateModalModelBase(IRigaOrdiniAppService rigaOrdiniAppService)
    {
        _rigaOrdiniAppService = rigaOrdiniAppService;
        RigaOrdine = new();
    }

    public virtual async Task OnGetAsync()
    {
        RigaOrdine = new RigaOrdineCreateViewModel();
        OrdineLookupList.AddRange((await _rigaOrdiniAppService.GetOrdineLookupAsync(new LookupRequestDto { MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount })).Items.Select(t => new SelectListItem(t.DisplayName, t.Id.ToString())).ToList());
        VarianteProdottoLookupList.AddRange((await _rigaOrdiniAppService.GetVarianteProdottoLookupAsync(new LookupRequestDto { MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount })).Items.Select(t => new SelectListItem(t.DisplayName, t.Id.ToString())).ToList());
        await Task.CompletedTask;
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        await _rigaOrdiniAppService.CreateAsync(ObjectMapper.Map<RigaOrdineCreateViewModel, RigaOrdineCreateDto>(RigaOrdine));
        return NoContent();
    }
}

public class RigaOrdineCreateViewModel : RigaOrdineCreateDto
{
}