using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using LFG.Shared;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using LFG.Indirizzi;

namespace LFG.Web.Pages.Indirizzi;

public abstract class EditModalModelBase : AbpPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public IndirizzoUpdateViewModel Indirizzo { get; set; }

    public List<SelectListItem> ClienteLookupList { get; set; } = new List<SelectListItem> { new SelectListItem(" — ", "") };

    protected IIndirizziAppService _indirizziAppService;

    public EditModalModelBase(IIndirizziAppService indirizziAppService)
    {
        _indirizziAppService = indirizziAppService;
        Indirizzo = new();
    }

    public virtual async Task OnGetAsync()
    {
        var indirizzoWithNavigationPropertiesDto = await _indirizziAppService.GetWithNavigationPropertiesAsync(Id);
        Indirizzo = ObjectMapper.Map<IndirizzoDto, IndirizzoUpdateViewModel>(indirizzoWithNavigationPropertiesDto.Indirizzo);
        ClienteLookupList.AddRange((await _indirizziAppService.GetClienteLookupAsync(new LookupRequestDto { MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount })).Items.Select(t => new SelectListItem(t.DisplayName, t.Id.ToString())).ToList());
    }

    public virtual async Task<NoContentResult> OnPostAsync()
    {
        await _indirizziAppService.UpdateAsync(Id, ObjectMapper.Map<IndirizzoUpdateViewModel, IndirizzoUpdateDto>(Indirizzo));
        return NoContent();
    }
}

public class IndirizzoUpdateViewModel : IndirizzoUpdateDto
{
}