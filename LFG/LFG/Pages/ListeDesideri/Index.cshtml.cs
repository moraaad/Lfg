using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using LFG.ListeDesideri;
using LFG.Shared;

namespace LFG.Web.Pages.ListeDesideri;

public abstract class IndexModelBase : AbpPageModel
{
    public DateTime? DataCreazioneFilterMin { get; set; }

    public DateTime? DataCreazioneFilterMax { get; set; }

    public string? NomeListaFilter { get; set; }

    [SelectItems(nameof(ClienteLookupList))]
    public Guid? ClienteIdFilter { get; set; }

    public List<SelectListItem> ClienteLookupList { get; set; } = new List<SelectListItem> { new SelectListItem(string.Empty, "") };

    protected IListeDesideriAppService _listeDesideriAppService;

    public IndexModelBase(IListeDesideriAppService listeDesideriAppService)
    {
        _listeDesideriAppService = listeDesideriAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ClienteLookupList.AddRange((await _listeDesideriAppService.GetClienteLookupAsync(new LookupRequestDto { MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount })).Items.Select(t => new SelectListItem(t.DisplayName, t.Id.ToString())).ToList());
        await Task.CompletedTask;
    }
}