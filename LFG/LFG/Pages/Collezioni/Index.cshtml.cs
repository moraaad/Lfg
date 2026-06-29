using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using LFG.Collezioni;
using LFG.Shared;

namespace LFG.Web.Pages.Collezioni;

public abstract class IndexModelBase : AbpPageModel
{
    public string? NomeFilter { get; set; }

    public string? StagioneFilter { get; set; }

    public DateTime? AnnoFilterMin { get; set; }

    public DateTime? AnnoFilterMax { get; set; }

    public string? SezioneFilter { get; set; }

    protected ICollezioniAppService _collezioniAppService;

    public IndexModelBase(ICollezioniAppService collezioniAppService)
    {
        _collezioniAppService = collezioniAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }
}