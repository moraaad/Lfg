using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using LFG.Shared;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using LFG.RigaOrdini;

namespace LFG.Web.Pages.RigaOrdini;

public abstract class EditModalModelBase : AbpPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public RigaOrdineUpdateViewModel RigaOrdine { get; set; }

    public List<SelectListItem> OrdineLookupList { get; set; } = new List<SelectListItem> { new SelectListItem(" — ", "") };
    public List<SelectListItem> VarianteProdottoLookupList { get; set; } = new List<SelectListItem> { new SelectListItem(" — ", "") };

    protected IRigaOrdiniAppService _rigaOrdiniAppService;

    public EditModalModelBase(IRigaOrdiniAppService rigaOrdiniAppService)
    {
        _rigaOrdiniAppService = rigaOrdiniAppService;
        RigaOrdine = new();
    }

    public virtual async Task OnGetAsync()
    {
        var rigaOrdineWithNavigationPropertiesDto = await _rigaOrdiniAppService.GetWithNavigationPropertiesAsync(Id);
        RigaOrdine = ObjectMapper.Map<RigaOrdineDto, RigaOrdineUpdateViewModel>(rigaOrdineWithNavigationPropertiesDto.RigaOrdine);
        OrdineLookupList.AddRange((await _rigaOrdiniAppService.GetOrdineLookupAsync(new LookupRequestDto { MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount })).Items.Select(t => new SelectListItem(t.DisplayName, t.Id.ToString())).ToList());
        VarianteProdottoLookupList.AddRange((await _rigaOrdiniAppService.GetVarianteProdottoLookupAsync(new LookupRequestDto { MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount })).Items.Select(t => new SelectListItem(t.DisplayName, t.Id.ToString())).ToList());
    }

    public virtual async Task<NoContentResult> OnPostAsync()
    {
        await _rigaOrdiniAppService.UpdateAsync(Id, ObjectMapper.Map<RigaOrdineUpdateViewModel, RigaOrdineUpdateDto>(RigaOrdine));
        return NoContent();
    }
}

public class RigaOrdineUpdateViewModel : RigaOrdineUpdateDto
{
}