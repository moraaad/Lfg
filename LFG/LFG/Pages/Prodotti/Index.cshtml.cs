using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using LFG.Prodotti;
using LFG.Shared;

namespace LFG.Web.Pages.Prodotti;

public abstract class IndexModelBase : AbpPageModel
{
    public string? NomeFilter { get; set; }

    public string? DescrizioneFilter { get; set; }

    public string? PrezzoFilter { get; set; }

    public string? CodiceSkuFilter { get; set; }

    public string? SezioneFilter { get; set; }

    [SelectItems(nameof(CategoriaLookupList))]
    public Guid? CategoriaIdFilter { get; set; }

    public List<SelectListItem> CategoriaLookupList { get; set; } = new List<SelectListItem> { new SelectListItem(string.Empty, "") };

    protected IProdottiAppService _prodottiAppService;

    public IndexModelBase(IProdottiAppService prodottiAppService)
    {
        _prodottiAppService = prodottiAppService;
    }

    public virtual async Task OnGetAsync()
    {
        CategoriaLookupList.AddRange((await _prodottiAppService.GetCategoriaLookupAsync(new LookupRequestDto { MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount })).Items.Select(t => new SelectListItem(t.DisplayName, t.Id.ToString())).ToList());
        await Task.CompletedTask;
    }
}