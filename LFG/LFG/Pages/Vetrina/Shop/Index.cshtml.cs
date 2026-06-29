using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LFG.Prodotti;
using LFG.VarianteProdotti;

namespace LFG.Pages.Vetrina.Shop;

[AllowAnonymous]
public class IndexModel : PageModel
{
    private readonly IProdottiAppService _prodottiAppService;
    private readonly IVarianteProdottiAppService _varianteAppService;

    public IndexModel(
        IProdottiAppService prodottiAppService,
        IVarianteProdottiAppService varianteAppService)
    {
        _prodottiAppService = prodottiAppService;
        _varianteAppService = varianteAppService;
    }

    [BindProperty(SupportsGet = true)]
    public string Sezione { get; set; } = "LFG";

    public record CapoVetrina(Guid Id, string Nome, string Prezzo, string Sezione, string UrlImmagine);

    public List<CapoVetrina> Capi { get; set; } = new();

    public bool SezioneValida =>
        string.Equals(Sezione, "LFG", StringComparison.OrdinalIgnoreCase) ||
        string.Equals(Sezione, "GLF", StringComparison.OrdinalIgnoreCase);

    public async Task<IActionResult> OnGetAsync()
    {
        if (!SezioneValida)
            Sezione = "LFG";

        var prodotti = await _prodottiAppService.GetListAsync(
            new GetProdottiInput { MaxResultCount = 100, Sezione = Sezione });

        var varianti = await _varianteAppService.GetListAsync(
            new GetVarianteProdottiInput { MaxResultCount = 1000 });

        foreach (var x in prodotti.Items)
        {
            var prod = x.Prodotto;

            var img = varianti.Items
                .Where(v => v.ProdottoId == prod.Id && !string.IsNullOrEmpty(v.UrlImmagine))
                .Select(v => v.UrlImmagine!)
                .FirstOrDefault() ?? "/images/placeholder.png";

            Capi.Add(new CapoVetrina(prod.Id, prod.Nome, prod.Prezzo, prod.Sezione, img));
        }

        return Page();
    }
}
