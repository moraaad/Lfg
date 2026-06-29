using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using LFG.Shared;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using LFG.Clienti;

namespace LFG.Web.Pages.Clienti;

public abstract class EditModalModelBase : AbpPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public ClienteUpdateViewModel Cliente { get; set; }

    protected IClientiAppService _clientiAppService;

    public EditModalModelBase(IClientiAppService clientiAppService)
    {
        _clientiAppService = clientiAppService;
        Cliente = new();
    }

    public virtual async Task OnGetAsync()
    {
        var cliente = await _clientiAppService.GetAsync(Id);
        Cliente = ObjectMapper.Map<ClienteDto, ClienteUpdateViewModel>(cliente);
    }

    public virtual async Task<NoContentResult> OnPostAsync()
    {
        await _clientiAppService.UpdateAsync(Id, ObjectMapper.Map<ClienteUpdateViewModel, ClienteUpdateDto>(Cliente));
        return NoContent();
    }
}

public class ClienteUpdateViewModel : ClienteUpdateDto
{
}