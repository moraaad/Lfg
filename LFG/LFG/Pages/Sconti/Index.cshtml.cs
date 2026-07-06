using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using LFG.Sconti;
using LFG.Shared;

namespace LFG.Web.Pages.Sconti;

public abstract class IndexModelBase : AbpPageModel
{
    public string? CodiceFilter { get; set; }

    public string? TipoFilter { get; set; }

    public decimal? ValoreFilterMin { get; set; }

    public decimal? ValoreFilterMax { get; set; }

    public int? LimiteUtilizziFilterMin { get; set; }

    public int? LimiteUtilizziFilterMax { get; set; }

    public DateTime? ValidoDalFilterMin { get; set; }

    public DateTime? ValidoDalFilterMax { get; set; }

    public DateTime? ValidoAlFilterMin { get; set; }

    public DateTime? ValidoAlFilterMax { get; set; }

    public string? SezioneFilter { get; set; }

    protected IScontiAppService _scontiAppService;

    public IndexModelBase(IScontiAppService scontiAppService)
    {
        _scontiAppService = scontiAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }
}