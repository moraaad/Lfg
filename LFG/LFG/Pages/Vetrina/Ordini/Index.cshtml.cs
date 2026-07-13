using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Volo.Abp.Domain.Repositories;
using LFG.VarianteProdotti;
using LFG.Ordini;
using LFG.RigaOrdini;
using LFG.Clienti;
using LFG.Sconti;
using LFG.Indirizzi;

namespace LFG.Pages.Vetrina.Ordini;

[AllowAnonymous]
public class IndexModel : PageModel
{
    private readonly IClientiAppService _clientiAppService;
    private readonly IRepository<Ordine, Guid> _ordineRepo;
    private readonly IRepository<RigaOrdine, Guid> _rigaOrdineRepo;
    private readonly IRepository<VarianteProdotto, Guid> _varianteRepo;
    private readonly IRepository<LFG.Prodotti.Prodotto, Guid> _prodottoRepo;
    private readonly IRepository<Sconto, Guid> _scontoRepo;
    private readonly IRepository<Indirizzo, Guid> _indirizzoRepo;

    public IndexModel(
        IClientiAppService clientiAppService,
        IRepository<Ordine, Guid> ordineRepo,
        IRepository<RigaOrdine, Guid> rigaOrdineRepo,
        IRepository<VarianteProdotto, Guid> varianteRepo,
        IRepository<LFG.Prodotti.Prodotto, Guid> prodottoRepo,
        IRepository<Sconto, Guid> scontoRepo,
        IRepository<Indirizzo, Guid> indirizzoRepo)
    {
        _clientiAppService = clientiAppService;
        _ordineRepo         = ordineRepo;
        _rigaOrdineRepo     = rigaOrdineRepo;
        _varianteRepo       = varianteRepo;
        _prodottoRepo       = prodottoRepo;
        _scontoRepo         = scontoRepo;
        _indirizzoRepo      = indirizzoRepo;
    }

    public bool Autenticato { get; set; }

    public record RigaVista(
        string Nome, string Taglia, string Colore, string UrlImmagine,
        decimal PrezzoUnitario, int Quantita, decimal Subtotale);

    public record OrdineVista(
        Guid Id, DateTime DataOrdine, decimal ImportoTotale, string? MetodoPagamento,
        string? Stato, string IndirizzoTesto, string? ScontoCodice, int TotalePezzi,
        List<RigaVista> Righe);

    public List<OrdineVista> Ordini { get; set; } = new();

    public async Task<IActionResult> OnGetAsync()
    {
        Autenticato = User.Identity?.IsAuthenticated == true;
        if (!Autenticato)
            return Page();

        var cliente = await _clientiAppService.GetClienteCorrenteAsync();
        if (cliente == null)
            return Page();

        // Solo ordini Confermati: le bozze (carrello) non compaiono qui
        var ordiniConfermati = (await _ordineRepo.GetListAsync(
                o => o.ClienteId == cliente.Id && o.Stato == "Confermato"))
            .OrderByDescending(o => o.DataOrdine)
            .ToList();

        if (!ordiniConfermati.Any())
            return Page();

        var ordineIds = ordiniConfermati.Select(o => o.Id).ToList();
        var tutteLeRighe = await _rigaOrdineRepo.GetListAsync(
            r => r.OrdineId.HasValue && ordineIds.Contains(r.OrdineId.Value));

        var varianteIds = tutteLeRighe
            .Where(r => r.VarianteProdottoId.HasValue)
            .Select(r => r.VarianteProdottoId!.Value)
            .Distinct()
            .ToList();
        var varianti = (await _varianteRepo.GetListAsync(v => varianteIds.Contains(v.Id))).ToDictionary(v => v.Id);

        var prodottoIds = varianti.Values.Select(v => v.ProdottoId).Distinct().ToList();
        var prodotti = (await _prodottoRepo.GetListAsync(p => prodottoIds.Contains(p.Id))).ToDictionary(p => p.Id);

        var scontoIds = ordiniConfermati
            .Where(o => o.ScontoId.HasValue)
            .Select(o => o.ScontoId!.Value)
            .Distinct()
            .ToList();
        var sconti = scontoIds.Any()
            ? (await _scontoRepo.GetListAsync(s => scontoIds.Contains(s.Id))).ToDictionary(s => s.Id)
            : new Dictionary<Guid, Sconto>();

        var indirizzoIds = ordiniConfermati
            .Where(o => o.IndirizzoId.HasValue)
            .Select(o => o.IndirizzoId!.Value)
            .Distinct()
            .ToList();
        var indirizzi = indirizzoIds.Any()
            ? (await _indirizzoRepo.GetListAsync(i => indirizzoIds.Contains(i.Id))).ToDictionary(i => i.Id)
            : new Dictionary<Guid, Indirizzo>();

        Ordini = ordiniConfermati
            .Select(o =>
            {
                var righeOrdine = tutteLeRighe.Where(r => r.OrdineId == o.Id).ToList();

                var righeVista = righeOrdine
                    .Where(r => r.VarianteProdottoId.HasValue && varianti.ContainsKey(r.VarianteProdottoId.Value))
                    .Select(r =>
                    {
                        var variante = varianti[r.VarianteProdottoId!.Value];
                        var prod = prodotti.GetValueOrDefault(variante.ProdottoId);
                        return new RigaVista(
                            prod?.Nome ?? "—",
                            variante.Taglia ?? "—",
                            variante.Colore ?? "—",
                            string.IsNullOrEmpty(variante.UrlImmagine) ? "/images/placeholder.png" : variante.UrlImmagine,
                            r.PrezzoUnitario,
                            r.Quantita,
                            r.PrezzoUnitario * r.Quantita);
                    })
                    .ToList();

                var scontoCodice = o.ScontoId.HasValue && sconti.TryGetValue(o.ScontoId.Value, out var sc)
                    ? sc.Codice
                    : null;

                string indirizzoTesto = "Indirizzo non disponibile";
                if (o.IndirizzoId.HasValue && indirizzi.TryGetValue(o.IndirizzoId.Value, out var ind))
                    indirizzoTesto = FormattaIndirizzo(ind);
                else if (!string.IsNullOrWhiteSpace(o.IndSpedizione))
                    indirizzoTesto = o.IndSpedizione;

                return new OrdineVista(
                    o.Id,
                    o.DataOrdine,
                    o.ImportoTotale,
                    o.MetodoPagamento,
                    o.Stato,
                    indirizzoTesto,
                    scontoCodice,
                    righeOrdine.Sum(r => r.Quantita),
                    righeVista);
            })
            .ToList();

        return Page();
    }

    private static string FormattaIndirizzo(Indirizzo indirizzo)
    {
        var testo = indirizzo.Via + ", " + indirizzo.Cap;
        if (!string.IsNullOrWhiteSpace(indirizzo.Citta))
            testo += " " + indirizzo.Citta;
        if (!string.IsNullOrWhiteSpace(indirizzo.Provincia))
            testo += " (" + indirizzo.Provincia + ")";
        if (!string.IsNullOrWhiteSpace(indirizzo.Paese))
            testo += ", " + indirizzo.Paese;
        return testo;
    }
}
