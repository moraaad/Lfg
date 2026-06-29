using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using LFG.Shared;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using LFG.Recensioni;

namespace LFG.Web.Pages.Recensioni;

public abstract class EditModalModelBase : AbpPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public RecensioneUpdateViewModel Recensione { get; set; }

    public List<SelectListItem> ClienteLookupList { get; set; } = new List<SelectListItem> { new SelectListItem(" — ", "") };
    public List<SelectListItem> ProdottoLookupList { get; set; } = new List<SelectListItem> { new SelectListItem(" — ", "") };

    protected IRecensioniAppService _recensioniAppService;

    public EditModalModelBase(IRecensioniAppService recensioniAppService)
    {
        _recensioniAppService = recensioniAppService;
        Recensione = new();
    }

    public virtual async Task OnGetAsync()
    {
        var recensioneWithNavigationPropertiesDto = await _recensioniAppService.GetWithNavigationPropertiesAsync(Id);
        Recensione = ObjectMapper.Map<RecensioneDto, RecensioneUpdateViewModel>(recensioneWithNavigationPropertiesDto.Recensione);
        ClienteLookupList.AddRange((await _recensioniAppService.GetClienteLookupAsync(new LookupRequestDto { MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount })).Items.Select(t => new SelectListItem(t.DisplayName, t.Id.ToString())).ToList());
        ProdottoLookupList.AddRange((await _recensioniAppService.GetProdottoLookupAsync(new LookupRequestDto { MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount })).Items.Select(t => new SelectListItem(t.DisplayName, t.Id.ToString())).ToList());
    }

    public virtual async Task<NoContentResult> OnPostAsync()
    {
        await _recensioniAppService.UpdateAsync(Id, ObjectMapper.Map<RecensioneUpdateViewModel, RecensioneUpdateDto>(Recensione));
        return NoContent();
    }
}

public class RecensioneUpdateViewModel : RecensioneUpdateDto
{
}