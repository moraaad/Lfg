using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using LFG.Shared;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LFG.Ordini;

namespace LFG.Web.Pages.Ordini;

public abstract class CreateModalModelBase : AbpPageModel
{
    [BindProperty]
    public OrdineCreateViewModel Ordine { get; set; }

    public List<SelectListItem> ClienteLookupList { get; set; } = new List<SelectListItem> { new SelectListItem(" — ", "") };
    public List<SelectListItem> ScontoLookupList { get; set; } = new List<SelectListItem> { new SelectListItem(" — ", "") };
    public List<SelectListItem> IndirizzoLookupList { get; set; } = new List<SelectListItem> { new SelectListItem(" — ", "") };

    protected IOrdiniAppService _ordiniAppService;

    public CreateModalModelBase(IOrdiniAppService ordiniAppService)
    {
        _ordiniAppService = ordiniAppService;
        Ordine = new();
    }

    public virtual async Task OnGetAsync()
    {
        Ordine = new OrdineCreateViewModel();
        ClienteLookupList.AddRange((await _ordiniAppService.GetClienteLookupAsync(new LookupRequestDto { MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount })).Items.Select(t => new SelectListItem(t.DisplayName, t.Id.ToString())).ToList());
        ScontoLookupList.AddRange((await _ordiniAppService.GetScontoLookupAsync(new LookupRequestDto { MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount })).Items.Select(t => new SelectListItem(t.DisplayName, t.Id.ToString())).ToList());
        IndirizzoLookupList.AddRange((await _ordiniAppService.GetIndirizzoLookupAsync(new LookupRequestDto { MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount })).Items.Select(t => new SelectListItem(t.DisplayName, t.Id.ToString())).ToList());
        await Task.CompletedTask;
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        await _ordiniAppService.CreateAsync(ObjectMapper.Map<OrdineCreateViewModel, OrdineCreateDto>(Ordine));
        return NoContent();
    }
}

public class OrdineCreateViewModel : OrdineCreateDto
{
}