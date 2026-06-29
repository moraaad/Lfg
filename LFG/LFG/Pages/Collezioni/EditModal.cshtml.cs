using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using LFG.Shared;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using LFG.Collezioni;

namespace LFG.Web.Pages.Collezioni;

public abstract class EditModalModelBase : AbpPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public CollezioneUpdateViewModel Collezione { get; set; }

    protected ICollezioniAppService _collezioniAppService;

    public EditModalModelBase(ICollezioniAppService collezioniAppService)
    {
        _collezioniAppService = collezioniAppService;
        Collezione = new();
    }

    public virtual async Task OnGetAsync()
    {
        var collezione = await _collezioniAppService.GetAsync(Id);
        Collezione = ObjectMapper.Map<CollezioneDto, CollezioneUpdateViewModel>(collezione);
    }

    public virtual async Task<NoContentResult> OnPostAsync()
    {
        await _collezioniAppService.UpdateAsync(Id, ObjectMapper.Map<CollezioneUpdateViewModel, CollezioneUpdateDto>(Collezione));
        return NoContent();
    }
}

public class CollezioneUpdateViewModel : CollezioneUpdateDto
{
}