using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using LFG.Shared;
using LFG.Collezioni;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using LFG.Prodotti;

namespace LFG.Web.Pages.Prodotti;

public abstract class EditModalModelBase : AbpPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public ProdottoUpdateViewModel Prodotto { get; set; }

    public List<CollezioneDto> Colleziones { get; set; }

    [BindProperty]
    public List<Guid> SelectedCollezionesIds { get; set; }

    public List<SelectListItem> CategoriaLookupList { get; set; } = new List<SelectListItem> { new SelectListItem(" — ", "") };

    protected IProdottiAppService _prodottiAppService;

    public EditModalModelBase(IProdottiAppService prodottiAppService)
    {
        _prodottiAppService = prodottiAppService;
        Prodotto = new();
    }

    public virtual async Task OnGetAsync()
    {
        var prodottoWithNavigationPropertiesDto = await _prodottiAppService.GetWithNavigationPropertiesAsync(Id);
        Prodotto = ObjectMapper.Map<ProdottoDto, ProdottoUpdateViewModel>(prodottoWithNavigationPropertiesDto.Prodotto);
        Colleziones = prodottoWithNavigationPropertiesDto.Colleziones;
        CategoriaLookupList.AddRange((await _prodottiAppService.GetCategoriaLookupAsync(new LookupRequestDto { MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount })).Items.Select(t => new SelectListItem(t.DisplayName, t.Id.ToString())).ToList());
    }

    public virtual async Task<NoContentResult> OnPostAsync()
    {
        Prodotto.CollezionesIds = SelectedCollezionesIds;
        await _prodottiAppService.UpdateAsync(Id, ObjectMapper.Map<ProdottoUpdateViewModel, ProdottoUpdateDto>(Prodotto));
        return NoContent();
    }
}

public class ProdottoUpdateViewModel : ProdottoUpdateDto
{
}