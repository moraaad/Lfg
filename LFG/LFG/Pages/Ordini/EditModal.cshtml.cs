using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using LFG.Shared;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using LFG.Ordini;

namespace LFG.Web.Pages.Ordini;

public abstract class EditModalModelBase : AbpPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public OrdineUpdateViewModel Ordine { get; set; }

    public List<SelectListItem> ClienteLookupList { get; set; } = new List<SelectListItem> { new SelectListItem(" — ", "") };
    public List<SelectListItem> ScontoLookupList { get; set; } = new List<SelectListItem> { new SelectListItem(" — ", "") };

    protected IOrdiniAppService _ordiniAppService;

    public EditModalModelBase(IOrdiniAppService ordiniAppService)
    {
        _ordiniAppService = ordiniAppService;
        Ordine = new();
    }

    public virtual async Task OnGetAsync()
    {
        var ordineWithNavigationPropertiesDto = await _ordiniAppService.GetWithNavigationPropertiesAsync(Id);
        Ordine = ObjectMapper.Map<OrdineDto, OrdineUpdateViewModel>(ordineWithNavigationPropertiesDto.Ordine);
        ClienteLookupList.AddRange((await _ordiniAppService.GetClienteLookupAsync(new LookupRequestDto { MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount })).Items.Select(t => new SelectListItem(t.DisplayName, t.Id.ToString())).ToList());
        ScontoLookupList.AddRange((await _ordiniAppService.GetScontoLookupAsync(new LookupRequestDto { MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount })).Items.Select(t => new SelectListItem(t.DisplayName, t.Id.ToString())).ToList());
    }

    public virtual async Task<NoContentResult> OnPostAsync()
    {
        await _ordiniAppService.UpdateAsync(Id, ObjectMapper.Map<OrdineUpdateViewModel, OrdineUpdateDto>(Ordine));
        return NoContent();
    }
}

public class OrdineUpdateViewModel : OrdineUpdateDto
{
}