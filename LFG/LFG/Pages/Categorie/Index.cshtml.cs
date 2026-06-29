using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using LFG.Categorie;
using LFG.Shared;

namespace LFG.Web.Pages.Categorie;

public abstract class IndexModelBase : AbpPageModel
{
    public string? NomeFilter { get; set; }

    public string? DescrizioneFilter { get; set; }

    public string? SezioneFilter { get; set; }

    protected ICategorieAppService _categorieAppService;

    public IndexModelBase(ICategorieAppService categorieAppService)
    {
        _categorieAppService = categorieAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }
}