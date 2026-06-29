using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using LFG.Shared;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LFG.ElementoListe;

namespace LFG.Web.Pages.ElementoListe;

public abstract class CreateModalModelBase : AbpPageModel
{
    [BindProperty]
    public ElementoListaCreateViewModel ElementoLista { get; set; }

    public List<SelectListItem> VarianteProdottoLookupListRequired { get; set; } = new List<SelectListItem> { };
    public List<SelectListItem> ListaDesideriLookupList { get; set; } = new List<SelectListItem> { new SelectListItem(" — ", "") };

    protected IElementoListeAppService _elementoListeAppService;

    public CreateModalModelBase(IElementoListeAppService elementoListeAppService)
    {
        _elementoListeAppService = elementoListeAppService;
        ElementoLista = new();
    }

    public virtual async Task OnGetAsync()
    {
        ElementoLista = new ElementoListaCreateViewModel();
        VarianteProdottoLookupListRequired.AddRange((await _elementoListeAppService.GetVarianteProdottoLookupAsync(new LookupRequestDto { MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount })).Items.Select(t => new SelectListItem(t.DisplayName, t.Id.ToString())).ToList());
        ListaDesideriLookupList.AddRange((await _elementoListeAppService.GetListaDesideriLookupAsync(new LookupRequestDto { MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount })).Items.Select(t => new SelectListItem(t.DisplayName, t.Id.ToString())).ToList());
        await Task.CompletedTask;
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        await _elementoListeAppService.CreateAsync(ObjectMapper.Map<ElementoListaCreateViewModel, ElementoListaCreateDto>(ElementoLista));
        return NoContent();
    }
}

public class ElementoListaCreateViewModel : ElementoListaCreateDto
{
}