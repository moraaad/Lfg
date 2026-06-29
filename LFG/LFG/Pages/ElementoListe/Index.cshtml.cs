using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using LFG.ElementoListe;
using LFG.Shared;

namespace LFG.Web.Pages.ElementoListe;

public abstract class IndexModelBase : AbpPageModel
{
    public DateTime? DataAggiuntaFilterMin { get; set; }

    public DateTime? DataAggiuntaFilterMax { get; set; }

    [SelectItems(nameof(VarianteProdottoLookupList))]
    public Guid VarianteProdottoIdFilter { get; set; }

    public List<SelectListItem> VarianteProdottoLookupList { get; set; } = new List<SelectListItem> { new SelectListItem(string.Empty, "") };
    [SelectItems(nameof(ListaDesideriLookupList))]
    public Guid? ListaDesideriIdFilter { get; set; }

    public List<SelectListItem> ListaDesideriLookupList { get; set; } = new List<SelectListItem> { new SelectListItem(string.Empty, "") };

    protected IElementoListeAppService _elementoListeAppService;

    public IndexModelBase(IElementoListeAppService elementoListeAppService)
    {
        _elementoListeAppService = elementoListeAppService;
    }

    public virtual async Task OnGetAsync()
    {
        VarianteProdottoLookupList.AddRange((await _elementoListeAppService.GetVarianteProdottoLookupAsync(new LookupRequestDto { MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount })).Items.Select(t => new SelectListItem(t.DisplayName, t.Id.ToString())).ToList());
        ListaDesideriLookupList.AddRange((await _elementoListeAppService.GetListaDesideriLookupAsync(new LookupRequestDto { MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount })).Items.Select(t => new SelectListItem(t.DisplayName, t.Id.ToString())).ToList());
        await Task.CompletedTask;
    }
}