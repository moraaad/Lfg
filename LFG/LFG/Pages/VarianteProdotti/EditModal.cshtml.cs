using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using LFG.Shared;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using LFG.VarianteProdotti;

namespace LFG.Web.Pages.VarianteProdotti;

public abstract class EditModalModelBase : AbpPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public VarianteProdottoUpdateViewModel VarianteProdotto { get; set; }

    protected IVarianteProdottiAppService _varianteProdottiAppService;

    public EditModalModelBase(IVarianteProdottiAppService varianteProdottiAppService)
    {
        _varianteProdottiAppService = varianteProdottiAppService;
        VarianteProdotto = new();
    }

    public virtual async Task OnGetAsync()
    {
        var varianteProdotto = await _varianteProdottiAppService.GetAsync(Id);
        VarianteProdotto = ObjectMapper.Map<VarianteProdottoDto, VarianteProdottoUpdateViewModel>(varianteProdotto);
    }

    public virtual async Task<NoContentResult> OnPostAsync()
    {
        await _varianteProdottiAppService.UpdateAsync(Id, ObjectMapper.Map<VarianteProdottoUpdateViewModel, VarianteProdottoUpdateDto>(VarianteProdotto));
        return NoContent();
    }
}

public class VarianteProdottoUpdateViewModel : VarianteProdottoUpdateDto
{
}