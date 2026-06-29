using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using LFG.Pagamenti;
using LFG.Shared;

namespace LFG.Web.Pages.Pagamenti;

public abstract class IndexModelBase : AbpPageModel
{
    public string? MetodoFilter { get; set; }

    public string? StatoFilter { get; set; }

    public decimal? ImportoFilterMin { get; set; }

    public decimal? ImportoFilterMax { get; set; }

    public DateTime? DataPagamentoFilterMin { get; set; }

    public DateTime? DataPagamentoFilterMax { get; set; }

    public string? IdTransazioneFilter { get; set; }

    [SelectItems(nameof(OrdineLookupList))]
    public Guid? OrdineIdFilter { get; set; }

    public List<SelectListItem> OrdineLookupList { get; set; } = new List<SelectListItem> { new SelectListItem(string.Empty, "") };

    protected IPagamentiAppService _pagamentiAppService;

    public IndexModelBase(IPagamentiAppService pagamentiAppService)
    {
        _pagamentiAppService = pagamentiAppService;
    }

    public virtual async Task OnGetAsync()
    {
        OrdineLookupList.AddRange((await _pagamentiAppService.GetOrdineLookupAsync(new LookupRequestDto { MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount })).Items.Select(t => new SelectListItem(t.DisplayName, t.Id.ToString())).ToList());
        await Task.CompletedTask;
    }
}