using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LFG.Prodotti;
using LFG.VarianteProdotti;

namespace LFG.Pages.Vetrina.Prodotto;

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

    public string Nome { get; set; } = "";
    public string Prezzo { get; set; } = "";
    public string Sezione { get; set; } = "";
    public string? Descrizione { get; set; }

    public record VarianteVista(
        Guid Id, string Taglia, string Colore, string Materiale,
        int QtaMagazzino, string UrlImmagine);

    public List<VarianteVista> Varianti { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(Guid id)
    {
        var dto = await _prodottiAppService.GetWithNavigationPropertiesAsync(id);
        if (dto?.Prodotto == null)
            return NotFound();

        var prod = dto.Prodotto;
        Nome = prod.Nome;
        Prezzo = prod.Prezzo;
        Sezione = prod.Sezione;
        Descrizione = prod.Descrizione;

        var varianti = await _varianteAppService.GetListByProdottoIdAsync(
            new GetVarianteProdottoListInput { ProdottoId = id, MaxResultCount = 100 });

        Varianti = varianti.Items
            .Select(v => new VarianteVista(
                v.Id,
                v.Taglia ?? "—",
                v.Colore ?? "—",
                v.Materiale ?? "—",
                v.QtaMagazzino,
                string.IsNullOrEmpty(v.UrlImmagine) ? "/images/placeholder.png" : v.UrlImmagine))
            .ToList();

        return Page();
    }
}
