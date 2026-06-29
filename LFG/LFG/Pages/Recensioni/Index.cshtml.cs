using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using LFG.Recensioni;
using LFG.Shared;

namespace LFG.Web.Pages.Recensioni;

public abstract class IndexModelBase : AbpPageModel
{
    public int? ValutazioneFilterMin { get; set; }

    public int? ValutazioneFilterMax { get; set; }

    public string? CommentoFilter { get; set; }

    public DateTime? DataRecensioneFilterMin { get; set; }

    public DateTime? DataRecensioneFilterMax { get; set; }

    [SelectItems(nameof(ClienteLookupList))]
    public Guid? ClienteIdFilter { get; set; }

    public List<SelectListItem> ClienteLookupList { get; set; } = new List<SelectListItem> { new SelectListItem(string.Empty, "") };
    [SelectItems(nameof(ProdottoLookupList))]
    public Guid? ProdottoIdFilter { get; set; }

    public List<SelectListItem> ProdottoLookupList { get; set; } = new List<SelectListItem> { new SelectListItem(string.Empty, "") };

    protected IRecensioniAppService _recensioniAppService;

    public IndexModelBase(IRecensioniAppService recensioniAppService)
    {
        _recensioniAppService = recensioniAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ClienteLookupList.AddRange((await _recensioniAppService.GetClienteLookupAsync(new LookupRequestDto { MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount })).Items.Select(t => new SelectListItem(t.DisplayName, t.Id.ToString())).ToList());
        ProdottoLookupList.AddRange((await _recensioniAppService.GetProdottoLookupAsync(new LookupRequestDto { MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount })).Items.Select(t => new SelectListItem(t.DisplayName, t.Id.ToString())).ToList());
        await Task.CompletedTask;
    }
}