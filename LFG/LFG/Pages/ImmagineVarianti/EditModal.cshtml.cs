using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using LFG.Shared;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using LFG.ImmagineVarianti;

namespace LFG.Web.Pages.ImmagineVarianti;

public abstract class EditModalModelBase : AbpPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public ImmagineVarianteUpdateViewModel ImmagineVariante { get; set; }

    public List<SelectListItem> VarianteProdottoLookupList { get; set; } = new List<SelectListItem> { new SelectListItem(" — ", "") };

    protected IImmagineVariantiAppService _immagineVariantiAppService;

    public EditModalModelBase(IImmagineVariantiAppService immagineVariantiAppService)
    {
        _immagineVariantiAppService = immagineVariantiAppService;
        ImmagineVariante = new();
    }

    public virtual async Task OnGetAsync()
    {
        var immagineVarianteWithNavigationPropertiesDto = await _immagineVariantiAppService.GetWithNavigationPropertiesAsync(Id);
        ImmagineVariante = ObjectMapper.Map<ImmagineVarianteDto, ImmagineVarianteUpdateViewModel>(immagineVarianteWithNavigationPropertiesDto.ImmagineVariante);
        VarianteProdottoLookupList.AddRange((await _immagineVariantiAppService.GetVarianteProdottoLookupAsync(new LookupRequestDto { MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount })).Items.Select(t => new SelectListItem(t.DisplayName, t.Id.ToString())).ToList());
    }

    public virtual async Task<NoContentResult> OnPostAsync()
    {
        await _immagineVariantiAppService.UpdateAsync(Id, ObjectMapper.Map<ImmagineVarianteUpdateViewModel, ImmagineVarianteUpdateDto>(ImmagineVariante));
        return NoContent();
    }
}

public class ImmagineVarianteUpdateViewModel : ImmagineVarianteUpdateDto
{
}