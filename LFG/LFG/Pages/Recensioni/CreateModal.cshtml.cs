using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using LFG.Shared;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LFG.Recensioni;

namespace LFG.Web.Pages.Recensioni;

public abstract class CreateModalModelBase : AbpPageModel
{
    [BindProperty]
    public RecensioneCreateViewModel Recensione { get; set; }

    public List<SelectListItem> ClienteLookupList { get; set; } = new List<SelectListItem> { new SelectListItem(" — ", "") };
    public List<SelectListItem> ProdottoLookupList { get; set; } = new List<SelectListItem> { new SelectListItem(" — ", "") };

    protected IRecensioniAppService _recensioniAppService;

    public CreateModalModelBase(IRecensioniAppService recensioniAppService)
    {
        _recensioniAppService = recensioniAppService;
        Recensione = new();
    }

    public virtual async Task OnGetAsync()
    {
        Recensione = new RecensioneCreateViewModel();
        ClienteLookupList.AddRange((await _recensioniAppService.GetClienteLookupAsync(new LookupRequestDto { MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount })).Items.Select(t => new SelectListItem(t.DisplayName, t.Id.ToString())).ToList());
        ProdottoLookupList.AddRange((await _recensioniAppService.GetProdottoLookupAsync(new LookupRequestDto { MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount })).Items.Select(t => new SelectListItem(t.DisplayName, t.Id.ToString())).ToList());
        await Task.CompletedTask;
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        await _recensioniAppService.CreateAsync(ObjectMapper.Map<RecensioneCreateViewModel, RecensioneCreateDto>(Recensione));
        return NoContent();
    }
}

public class RecensioneCreateViewModel : RecensioneCreateDto
{
}