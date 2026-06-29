using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using LFG.Indirizzi;
using LFG.Shared;

namespace LFG.Web.Pages.Indirizzi;

public abstract class IndexModelBase : AbpPageModel
{
    public string? PaeseFilter { get; set; }

    public string? CittaFilter { get; set; }

    public string? ProvinciaFilter { get; set; }

    public string? ViaFilter { get; set; }

    public string? CapFilter { get; set; }

    [SelectItems(nameof(ClienteLookupList))]
    public Guid? ClienteIdFilter { get; set; }

    public List<SelectListItem> ClienteLookupList { get; set; } = new List<SelectListItem> { new SelectListItem(string.Empty, "") };

    protected IIndirizziAppService _indirizziAppService;

    public IndexModelBase(IIndirizziAppService indirizziAppService)
    {
        _indirizziAppService = indirizziAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ClienteLookupList.AddRange((await _indirizziAppService.GetClienteLookupAsync(new LookupRequestDto { MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount })).Items.Select(t => new SelectListItem(t.DisplayName, t.Id.ToString())).ToList());
        await Task.CompletedTask;
    }
}