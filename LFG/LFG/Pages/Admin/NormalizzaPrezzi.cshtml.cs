using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LFG.Permissions;
using LFG.Prodotti;

namespace LFG.Pages.Admin;

[Authorize(LFGPermissions.Prodotti.Edit)]
public class NormalizzaPrezziModel : PageModel
{
    private readonly IProdottiAppService _prodottiAppService;

    public NormalizzaPrezziModel(IProdottiAppService prodottiAppService)
    {
        _prodottiAppService = prodottiAppService;
    }

    public PrezzoNormalizzazioneResultDto? Risultato { get; private set; }

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        Risultato = await _prodottiAppService.NormalizzaPrezziAsync();
        return Page();
    }
}
