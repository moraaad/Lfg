using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using LFG.Shared;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LFG.ListeDesideri;

namespace LFG.Web.Pages.ListeDesideri;

public abstract class CreateModalModelBase : AbpPageModel
{
    [BindProperty]
    public ListaDesideriCreateViewModel ListaDesideri { get; set; }

    public List<SelectListItem> ClienteLookupList { get; set; } = new List<SelectListItem> { new SelectListItem(" — ", "") };

    protected IListeDesideriAppService _listeDesideriAppService;

    public CreateModalModelBase(IListeDesideriAppService listeDesideriAppService)
    {
        _listeDesideriAppService = listeDesideriAppService;
        ListaDesideri = new();
    }

    public virtual async Task OnGetAsync()
    {
        ListaDesideri = new ListaDesideriCreateViewModel();
        ClienteLookupList.AddRange((await _listeDesideriAppService.GetClienteLookupAsync(new LookupRequestDto { MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount })).Items.Select(t => new SelectListItem(t.DisplayName, t.Id.ToString())).ToList());
        await Task.CompletedTask;
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        await _listeDesideriAppService.CreateAsync(ObjectMapper.Map<ListaDesideriCreateViewModel, ListaDesideriCreateDto>(ListaDesideri));
        return NoContent();
    }
}

public class ListaDesideriCreateViewModel : ListaDesideriCreateDto
{
}