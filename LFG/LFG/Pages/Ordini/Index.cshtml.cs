using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using LFG.Ordini;
using LFG.Shared;

namespace LFG.Web.Pages.Ordini;

public abstract class IndexModelBase : AbpPageModel
{
    public DateTime? DataOrdineFilterMin { get; set; }

    public DateTime? DataOrdineFilterMax { get; set; }

    public string? StatoFilter { get; set; }

    public decimal? ImportoTotaleFilterMin { get; set; }

    public decimal? ImportoTotaleFilterMax { get; set; }

    public string? IndSpedizioneFilter { get; set; }

    public string? MetodoPagamentoFilter { get; set; }

    [SelectItems(nameof(ClienteLookupList))]
    public Guid? ClienteIdFilter { get; set; }

    public List<SelectListItem> ClienteLookupList { get; set; } = new List<SelectListItem> { new SelectListItem(string.Empty, "") };
    [SelectItems(nameof(ScontoLookupList))]
    public Guid? ScontoIdFilter { get; set; }

    public List<SelectListItem> ScontoLookupList { get; set; } = new List<SelectListItem> { new SelectListItem(string.Empty, "") };

    protected IOrdiniAppService _ordiniAppService;

    public IndexModelBase(IOrdiniAppService ordiniAppService)
    {
        _ordiniAppService = ordiniAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ClienteLookupList.AddRange((await _ordiniAppService.GetClienteLookupAsync(new LookupRequestDto { MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount })).Items.Select(t => new SelectListItem(t.DisplayName, t.Id.ToString())).ToList());
        ScontoLookupList.AddRange((await _ordiniAppService.GetScontoLookupAsync(new LookupRequestDto { MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount })).Items.Select(t => new SelectListItem(t.DisplayName, t.Id.ToString())).ToList());
        await Task.CompletedTask;
    }
}