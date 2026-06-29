using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using LFG.Shared;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using LFG.ElementoListe;

namespace LFG.Web.Pages.ElementoListe;

public abstract class EditModalModelBase : AbpPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public ElementoListaUpdateViewModel ElementoLista { get; set; }

    public List<SelectListItem> VarianteProdottoLookupListRequired { get; set; } = new List<SelectListItem> { };
    public List<SelectListItem> ListaDesideriLookupList { get; set; } = new List<SelectListItem> { new SelectListItem(" — ", "") };

    protected IElementoListeAppService _elementoListeAppService;

    public EditModalModelBase(IElementoListeAppService elementoListeAppService)
    {
        _elementoListeAppService = elementoListeAppService;
        ElementoLista = new();
    }

    public virtual async Task OnGetAsync()
    {
        var elementoListaWithNavigationPropertiesDto = await _elementoListeAppService.GetWithNavigationPropertiesAsync(Id);
        ElementoLista = ObjectMapper.Map<ElementoListaDto, ElementoListaUpdateViewModel>(elementoListaWithNavigationPropertiesDto.ElementoLista);
        VarianteProdottoLookupListRequired.AddRange((await _elementoListeAppService.GetVarianteProdottoLookupAsync(new LookupRequestDto { MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount })).Items.Select(t => new SelectListItem(t.DisplayName, t.Id.ToString())).ToList());
        ListaDesideriLookupList.AddRange((await _elementoListeAppService.GetListaDesideriLookupAsync(new LookupRequestDto { MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount })).Items.Select(t => new SelectListItem(t.DisplayName, t.Id.ToString())).ToList());
    }

    public virtual async Task<NoContentResult> OnPostAsync()
    {
        await _elementoListeAppService.UpdateAsync(Id, ObjectMapper.Map<ElementoListaUpdateViewModel, ElementoListaUpdateDto>(ElementoLista));
        return NoContent();
    }
}

public class ElementoListaUpdateViewModel : ElementoListaUpdateDto
{
}