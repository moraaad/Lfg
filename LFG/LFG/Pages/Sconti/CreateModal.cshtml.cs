using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using LFG.Shared;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LFG.Sconti;

namespace LFG.Web.Pages.Sconti;

public abstract class CreateModalModelBase : AbpPageModel
{
    [BindProperty]
    public ScontoCreateViewModel Sconto { get; set; }

    protected IScontiAppService _scontiAppService;

    public CreateModalModelBase(IScontiAppService scontiAppService)
    {
        _scontiAppService = scontiAppService;
        Sconto = new();
    }

    public virtual async Task OnGetAsync()
    {
        Sconto = new ScontoCreateViewModel();
        await Task.CompletedTask;
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        await _scontiAppService.CreateAsync(ObjectMapper.Map<ScontoCreateViewModel, ScontoCreateDto>(Sconto));
        return NoContent();
    }
}

public class ScontoCreateViewModel : ScontoCreateDto
{
}