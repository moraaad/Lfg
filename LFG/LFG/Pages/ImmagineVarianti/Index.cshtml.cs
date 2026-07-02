using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using LFG.ImmagineVarianti;
using LFG.Shared;

namespace LFG.Web.Pages.ImmagineVarianti;

public abstract class IndexModelBase : AbpPageModel
{
    public string? UrlFilter { get; set; }

    public int? OrdineFilterMin { get; set; }

    public int? OrdineFilterMax { get; set; }

    [SelectItems(nameof(VarianteProdottoLookupList))]
    public Guid? VarianteProdottoIdFilter { get; set; }

    public List<SelectListItem> VarianteProdottoLookupList { get; set; } = new List<SelectListItem> { new SelectListItem(string.Empty, "") };

    protected IImmagineVariantiAppService _immagineVariantiAppService;

    public IndexModelBase(IImmagineVariantiAppService immagineVariantiAppService)
    {
        _immagineVariantiAppService = immagineVariantiAppService;
    }

    public virtual async Task OnGetAsync()
    {
        VarianteProdottoLookupList.AddRange((await _immagineVariantiAppService.GetVarianteProdottoLookupAsync(new LookupRequestDto { MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount })).Items.Select(t => new SelectListItem(t.DisplayName, t.Id.ToString())).ToList());
        await Task.CompletedTask;
    }
}