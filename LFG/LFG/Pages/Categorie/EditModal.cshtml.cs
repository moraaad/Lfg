using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using LFG.Shared;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using LFG.Categorie;

namespace LFG.Web.Pages.Categorie;

public abstract class EditModalModelBase : AbpPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public CategoriaUpdateViewModel Categoria { get; set; }

    protected ICategorieAppService _categorieAppService;

    public EditModalModelBase(ICategorieAppService categorieAppService)
    {
        _categorieAppService = categorieAppService;
        Categoria = new();
    }

    public virtual async Task OnGetAsync()
    {
        var categoria = await _categorieAppService.GetAsync(Id);
        Categoria = ObjectMapper.Map<CategoriaDto, CategoriaUpdateViewModel>(categoria);
    }

    public virtual async Task<NoContentResult> OnPostAsync()
    {
        await _categorieAppService.UpdateAsync(Id, ObjectMapper.Map<CategoriaUpdateViewModel, CategoriaUpdateDto>(Categoria));
        return NoContent();
    }
}

public class CategoriaUpdateViewModel : CategoriaUpdateDto
{
}