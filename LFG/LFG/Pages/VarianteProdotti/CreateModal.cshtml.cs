using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using LFG.Shared;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LFG.VarianteProdotti;

namespace LFG.Web.Pages.VarianteProdotti;

public abstract class CreateModalModelBase : AbpPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid ProdottoId { get; set; }

    [BindProperty]
    public VarianteProdottoCreateViewModel VarianteProdotto { get; set; }

    protected IVarianteProdottiAppService _varianteProdottiAppService;

    public CreateModalModelBase(IVarianteProdottiAppService varianteProdottiAppService)
    {
        _varianteProdottiAppService = varianteProdottiAppService;
        VarianteProdotto = new();
    }

    public virtual async Task OnGetAsync()
    {
        VarianteProdotto = new VarianteProdottoCreateViewModel();
        await Task.CompletedTask;
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        VarianteProdotto.ProdottoId = ProdottoId;
        await _varianteProdottiAppService.CreateAsync(ObjectMapper.Map<VarianteProdottoCreateViewModel, VarianteProdottoCreateDto>(VarianteProdotto));
        return NoContent();
    }
}

public class VarianteProdottoCreateViewModel : VarianteProdottoCreateDto
{
}