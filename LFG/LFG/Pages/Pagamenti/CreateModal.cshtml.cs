using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using LFG.Shared;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LFG.Pagamenti;

namespace LFG.Web.Pages.Pagamenti;

public abstract class CreateModalModelBase : AbpPageModel
{
    [BindProperty]
    public PagamentoCreateViewModel Pagamento { get; set; }

    public List<SelectListItem> OrdineLookupList { get; set; } = new List<SelectListItem> { new SelectListItem(" — ", "") };

    protected IPagamentiAppService _pagamentiAppService;

    public CreateModalModelBase(IPagamentiAppService pagamentiAppService)
    {
        _pagamentiAppService = pagamentiAppService;
        Pagamento = new();
    }

    public virtual async Task OnGetAsync()
    {
        Pagamento = new PagamentoCreateViewModel();
        OrdineLookupList.AddRange((await _pagamentiAppService.GetOrdineLookupAsync(new LookupRequestDto { MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount })).Items.Select(t => new SelectListItem(t.DisplayName, t.Id.ToString())).ToList());
        await Task.CompletedTask;
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        await _pagamentiAppService.CreateAsync(ObjectMapper.Map<PagamentoCreateViewModel, PagamentoCreateDto>(Pagamento));
        return NoContent();
    }
}

public class PagamentoCreateViewModel : PagamentoCreateDto
{
}