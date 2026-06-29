using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using LFG.Shared;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using LFG.Sconti;

namespace LFG.Web.Pages.Sconti;

public abstract class EditModalModelBase : AbpPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public ScontoUpdateViewModel Sconto { get; set; }

    protected IScontiAppService _scontiAppService;

    public EditModalModelBase(IScontiAppService scontiAppService)
    {
        _scontiAppService = scontiAppService;
        Sconto = new();
    }

    public virtual async Task OnGetAsync()
    {
        var sconto = await _scontiAppService.GetAsync(Id);
        Sconto = ObjectMapper.Map<ScontoDto, ScontoUpdateViewModel>(sconto);
    }

    public virtual async Task<NoContentResult> OnPostAsync()
    {
        await _scontiAppService.UpdateAsync(Id, ObjectMapper.Map<ScontoUpdateViewModel, ScontoUpdateDto>(Sconto));
        return NoContent();
    }
}

public class ScontoUpdateViewModel : ScontoUpdateDto
{
}