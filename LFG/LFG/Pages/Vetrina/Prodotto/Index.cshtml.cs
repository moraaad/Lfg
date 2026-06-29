using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Volo.Abp.Domain.Repositories;
using LFG.Prodotti;
using LFG.VarianteProdotti;

namespace LFG.Pages.Vetrina.Prodotto;

[AllowAnonymous]
public class IndexModel : PageModel
{
    private readonly IRepository<LFG.Prodotti.Prodotto, Guid> _prodottoRepo;
    private readonly IRepository<VarianteProdotto, Guid> _varianteRepo;

    public IndexModel(
        IRepository<LFG.Prodotti.Prodotto, Guid> prodottoRepo,
        IRepository<VarianteProdotto, Guid> varianteRepo)
    {
        _prodottoRepo = prodottoRepo;
        _varianteRepo = varianteRepo;
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
        var prod = await _prodottoRepo.FindAsync(id);
        if (prod == null)
            return NotFound();

        Nome = prod.Nome;
        Prezzo = prod.Prezzo;
        Sezione = prod.Sezione;
        Descrizione = prod.Descrizione;

        var varianti = await _varianteRepo.GetListAsync(v => v.ProdottoId == id);

        Varianti = varianti
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
