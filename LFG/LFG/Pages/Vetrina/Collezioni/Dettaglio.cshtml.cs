using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Volo.Abp.Domain.Repositories;
using LFG.Collezioni;
using LFG.Prodotti;
using LFG.VarianteProdotti;
using LFG.Helpers;

namespace LFG.Pages.Vetrina.Collezioni;

[AllowAnonymous]
public class DettaglioModel : PageModel
{
    private readonly IRepository<Collezione, Guid> _collezioneRepo;
    private readonly IRepository<ProdottoColleziones> _prodottoColleezioniRepo;
    private readonly IRepository<LFG.Prodotti.Prodotto, Guid> _prodottoRepo;
    private readonly IRepository<VarianteProdotto, Guid> _varianteRepo;

    public DettaglioModel(
        IRepository<Collezione, Guid> collezioneRepo,
        IRepository<ProdottoColleziones> prodottoColleezioniRepo,
        IRepository<LFG.Prodotti.Prodotto, Guid> prodottoRepo,
        IRepository<VarianteProdotto, Guid> varianteRepo)
    {
        _collezioneRepo = collezioneRepo;
        _prodottoColleezioniRepo = prodottoColleezioniRepo;
        _prodottoRepo = prodottoRepo;
        _varianteRepo = varianteRepo;
    }

    public string Nome { get; set; } = "";
    public string Stagione { get; set; } = "";
    public int Anno { get; set; }
    public string Sezione { get; set; } = "";

    public record ProdottoVista(Guid Id, string Nome, string Prezzo, string UrlImmagine);

    public List<ProdottoVista> Prodotti { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(Guid id)
    {
        var collezione = await _collezioneRepo.FindAsync(id);
        if (collezione == null)
            return NotFound();

        Nome = collezione.Nome;
        Stagione = collezione.Stagione;
        Anno = collezione.Anno.Year;
        Sezione = collezione.Sezione;

        // Ponte N:M: Collezione -> Collezione_Prodotto -> Prodotto
        var link = await _prodottoColleezioniRepo.GetListAsync(pc => pc.CollezioneId == id);
        var prodottoIds = link.Select(pc => pc.ProdottoId).ToList();

        if (prodottoIds.Any())
        {
            var prodotti = await _prodottoRepo.GetListAsync(p => prodottoIds.Contains(p.Id));
            var varianti = await _varianteRepo.GetListAsync(v => prodottoIds.Contains(v.ProdottoId));

            Prodotti = prodotti
                .Select(p =>
                {
                    var img = varianti
                        .Where(v => v.ProdottoId == p.Id && !string.IsNullOrEmpty(v.UrlImmagine))
                        .Select(v => v.UrlImmagine!)
                        .FirstOrDefault() ?? "/images/placeholder.png";

                    return new ProdottoVista(p.Id, p.Nome, PrezzoHelper.ToCanonico(PrezzoHelper.Parse(p.Prezzo)), img);
                })
                .ToList();
        }

        return Page();
    }
}
