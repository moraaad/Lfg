using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using LFG.Clienti;
using LFG.Shared;

namespace LFG.Web.Pages.Clienti;

public abstract class IndexModelBase : AbpPageModel
{
    public string? NomeFilter { get; set; }

    public string? CognomeFilter { get; set; }

    public string? GenereFilter { get; set; }

    public DateTime? DataNascitaFilterMin { get; set; }

    public DateTime? DataNascitaFilterMax { get; set; }

    public string? EmailFilter { get; set; }

    public string? TelefonoFilter { get; set; }

    public string? SezioneFilter { get; set; }

    public string? NazionalitaFilter { get; set; }

    public string? UserIdFilter { get; set; }

    protected IClientiAppService _clientiAppService;

    public IndexModelBase(IClientiAppService clientiAppService)
    {
        _clientiAppService = clientiAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }
}