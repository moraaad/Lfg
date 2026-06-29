using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using LFG.Shared;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LFG.Indirizzi;

namespace LFG.Web.Pages.Indirizzi;

public abstract class CreateModalModelBase : AbpPageModel
{
    [BindProperty]
    public IndirizzoCreateViewModel Indirizzo { get; set; }

    public List<SelectListItem> ClienteLookupList { get; set; } = new List<SelectListItem> { new SelectListItem(" — ", "") };

    protected IIndirizziAppService _indirizziAppService;

    public CreateModalModelBase(IIndirizziAppService indirizziAppService)
    {
        _indirizziAppService = indirizziAppService;
        Indirizzo = new();
    }

    public virtual async Task OnGetAsync()
    {
        Indirizzo = new IndirizzoCreateViewModel();
        ClienteLookupList.AddRange((await _indirizziAppService.GetClienteLookupAsync(new LookupRequestDto { MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount })).Items.Select(t => new SelectListItem(t.DisplayName, t.Id.ToString())).ToList());
        await Task.CompletedTask;
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        await _indirizziAppService.CreateAsync(ObjectMapper.Map<IndirizzoCreateViewModel, IndirizzoCreateDto>(Indirizzo));
        return NoContent();
    }
}

public class IndirizzoCreateViewModel : IndirizzoCreateDto
{
}