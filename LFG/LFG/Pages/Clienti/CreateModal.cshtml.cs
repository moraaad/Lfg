using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using LFG.Shared;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LFG.Clienti;

namespace LFG.Web.Pages.Clienti;

public abstract class CreateModalModelBase : AbpPageModel
{
    [BindProperty]
    public ClienteCreateViewModel Cliente { get; set; }

    protected IClientiAppService _clientiAppService;

    public CreateModalModelBase(IClientiAppService clientiAppService)
    {
        _clientiAppService = clientiAppService;
        Cliente = new();
    }

    public virtual async Task OnGetAsync()
    {
        Cliente = new ClienteCreateViewModel();
        await Task.CompletedTask;
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        await _clientiAppService.CreateAsync(ObjectMapper.Map<ClienteCreateViewModel, ClienteCreateDto>(Cliente));
        return NoContent();
    }
}

public class ClienteCreateViewModel : ClienteCreateDto
{
}