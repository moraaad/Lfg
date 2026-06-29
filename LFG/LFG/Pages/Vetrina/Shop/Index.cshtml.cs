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

namespace LFG.Pages.Vetrina.Shop;

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

        var prodotti = await _prodottoRepo.GetListAsync(p => p.Sezione == Sezione);
        var varianti = await _varianteRepo.GetListAsync();

        foreach (var prod in prodotti.Take(100))
        {
            var img = varianti
                .Where(v => v.ProdottoId == prod.Id && !string.IsNullOrEmpty(v.UrlImmagine))
                .Select(v => v.UrlImmagine!)
                .FirstOrDefault() ?? "/images/placeholder.png";

            Capi.Add(new CapoVetrina(prod.Id, prod.Nome, prod.Prezzo, prod.Sezione, img));
        }

        return Page();
    }
}
