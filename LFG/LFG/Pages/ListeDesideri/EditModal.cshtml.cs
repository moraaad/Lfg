using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using LFG.Shared;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using LFG.ListeDesideri;

namespace LFG.Web.Pages.ListeDesideri;

public abstract class EditModalModelBase : AbpPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public ListaDesideriUpdateViewModel ListaDesideri { get; set; }

    public List<SelectListItem> ClienteLookupList { get; set; } = new List<SelectListItem> { new SelectListItem(" — ", "") };

    protected IListeDesideriAppService _listeDesideriAppService;

    public EditModalModelBase(IListeDesideriAppService listeDesideriAppService)
    {
        _listeDesideriAppService = listeDesideriAppService;
        ListaDesideri = new();
    }

    public virtual async Task OnGetAsync()
    {
        var listaDesideriWithNavigationPropertiesDto = await _listeDesideriAppService.GetWithNavigationPropertiesAsync(Id);
        ListaDesideri = ObjectMapper.Map<ListaDesideriDto, ListaDesideriUpdateViewModel>(listaDesideriWithNavigationPropertiesDto.ListaDesideri);
        ClienteLookupList.AddRange((await _listeDesideriAppService.GetClienteLookupAsync(new LookupRequestDto { MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount })).Items.Select(t => new SelectListItem(t.DisplayName, t.Id.ToString())).ToList());
    }

    public virtual async Task<NoContentResult> OnPostAsync()
    {
        await _listeDesideriAppService.UpdateAsync(Id, ObjectMapper.Map<ListaDesideriUpdateViewModel, ListaDesideriUpdateDto>(ListaDesideri));
        return NoContent();
    }
}

public class ListaDesideriUpdateViewModel : ListaDesideriUpdateDto
{
}