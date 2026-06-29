using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using LFG.Shared;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LFG.Collezioni;

namespace LFG.Web.Pages.Collezioni;

public abstract class CreateModalModelBase : AbpPageModel
{
    [BindProperty]
    public CollezioneCreateViewModel Collezione { get; set; }

    protected ICollezioniAppService _collezioniAppService;

    public CreateModalModelBase(ICollezioniAppService collezioniAppService)
    {
        _collezioniAppService = collezioniAppService;
        Collezione = new();
    }

    public virtual async Task OnGetAsync()
    {
        Collezione = new CollezioneCreateViewModel();
        await Task.CompletedTask;
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        await _collezioniAppService.CreateAsync(ObjectMapper.Map<CollezioneCreateViewModel, CollezioneCreateDto>(Collezione));
        return NoContent();
    }
}

public class CollezioneCreateViewModel : CollezioneCreateDto
{
}