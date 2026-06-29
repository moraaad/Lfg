using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using LFG.Shared;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using LFG.Pagamenti;

namespace LFG.Web.Pages.Pagamenti;

public abstract class EditModalModelBase : AbpPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public PagamentoUpdateViewModel Pagamento { get; set; }

    public List<SelectListItem> OrdineLookupList { get; set; } = new List<SelectListItem> { new SelectListItem(" — ", "") };

    protected IPagamentiAppService _pagamentiAppService;

    public EditModalModelBase(IPagamentiAppService pagamentiAppService)
    {
        _pagamentiAppService = pagamentiAppService;
        Pagamento = new();
    }

    public virtual async Task OnGetAsync()
    {
        var pagamentoWithNavigationPropertiesDto = await _pagamentiAppService.GetWithNavigationPropertiesAsync(Id);
        Pagamento = ObjectMapper.Map<PagamentoDto, PagamentoUpdateViewModel>(pagamentoWithNavigationPropertiesDto.Pagamento);
        OrdineLookupList.AddRange((await _pagamentiAppService.GetOrdineLookupAsync(new LookupRequestDto { MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount })).Items.Select(t => new SelectListItem(t.DisplayName, t.Id.ToString())).ToList());
    }

    public virtual async Task<NoContentResult> OnPostAsync()
    {
        await _pagamentiAppService.UpdateAsync(Id, ObjectMapper.Map<PagamentoUpdateViewModel, PagamentoUpdateDto>(Pagamento));
        return NoContent();
    }
}

public class PagamentoUpdateViewModel : PagamentoUpdateDto
{
}