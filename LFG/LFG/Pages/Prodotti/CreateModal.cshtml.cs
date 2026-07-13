using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using LFG.Shared;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LFG.Prodotti;

namespace LFG.Web.Pages.Prodotti;

public abstract class CreateModalModelBase : AbpPageModel
{
    [BindProperty]
    public ProdottoCreateViewModel Prodotto { get; set; }

    public List<SelectListItem> CategoriaLookupList { get; set; } = new List<SelectListItem> { new SelectListItem(" — ", "") };
    public List<SelectListItem> CollezioneLookupList { get; set; } = new List<SelectListItem> { new SelectListItem(" — ", "") };

    protected IProdottiAppService _prodottiAppService;

    public CreateModalModelBase(IProdottiAppService prodottiAppService)
    {
        _prodottiAppService = prodottiAppService;
        Prodotto = new();
    }

    public virtual async Task OnGetAsync()
    {
        Prodotto = new ProdottoCreateViewModel();
        CategoriaLookupList.AddRange((await _prodottiAppService.GetCategoriaLookupAsync(new LookupRequestDto { MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount })).Items.Select(t => new SelectListItem(t.DisplayName, t.Id.ToString())).ToList());
        CollezioneLookupList.AddRange((await _prodottiAppService.GetCollezioneLookupAsync(new LookupRequestDto { MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount })).Items.Select(t => new SelectListItem(t.DisplayName, t.Id.ToString())).ToList());
        await Task.CompletedTask;
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        await _prodottiAppService.CreateAsync(ObjectMapper.Map<ProdottoCreateViewModel, ProdottoCreateDto>(Prodotto));
        return NoContent();
    }
}

public class ProdottoCreateViewModel : ProdottoCreateDto
{
}