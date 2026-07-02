using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using LFG.Shared;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LFG.ImmagineVarianti;

namespace LFG.Web.Pages.ImmagineVarianti;

public abstract class CreateModalModelBase : AbpPageModel
{
    [BindProperty]
    public ImmagineVarianteCreateViewModel ImmagineVariante { get; set; }

    public List<SelectListItem> VarianteProdottoLookupList { get; set; } = new List<SelectListItem> { new SelectListItem(" — ", "") };

    protected IImmagineVariantiAppService _immagineVariantiAppService;

    public CreateModalModelBase(IImmagineVariantiAppService immagineVariantiAppService)
    {
        _immagineVariantiAppService = immagineVariantiAppService;
        ImmagineVariante = new();
    }

    public virtual async Task OnGetAsync()
    {
        ImmagineVariante = new ImmagineVarianteCreateViewModel();
        VarianteProdottoLookupList.AddRange((await _immagineVariantiAppService.GetVarianteProdottoLookupAsync(new LookupRequestDto { MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount })).Items.Select(t => new SelectListItem(t.DisplayName, t.Id.ToString())).ToList());
        await Task.CompletedTask;
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        await _immagineVariantiAppService.CreateAsync(ObjectMapper.Map<ImmagineVarianteCreateViewModel, ImmagineVarianteCreateDto>(ImmagineVariante));
        return NoContent();
    }
}

public class ImmagineVarianteCreateViewModel : ImmagineVarianteCreateDto
{
}