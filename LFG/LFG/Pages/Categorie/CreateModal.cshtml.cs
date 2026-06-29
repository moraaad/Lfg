using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using LFG.Shared;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LFG.Categorie;

namespace LFG.Web.Pages.Categorie;

public abstract class CreateModalModelBase : AbpPageModel
{
    [BindProperty]
    public CategoriaCreateViewModel Categoria { get; set; }

    protected ICategorieAppService _categorieAppService;

    public CreateModalModelBase(ICategorieAppService categorieAppService)
    {
        _categorieAppService = categorieAppService;
        Categoria = new();
    }

    public virtual async Task OnGetAsync()
    {
        Categoria = new CategoriaCreateViewModel();
        await Task.CompletedTask;
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        await _categorieAppService.CreateAsync(ObjectMapper.Map<CategoriaCreateViewModel, CategoriaCreateDto>(Categoria));
        return NoContent();
    }
}

public class CategoriaCreateViewModel : CategoriaCreateDto
{
}