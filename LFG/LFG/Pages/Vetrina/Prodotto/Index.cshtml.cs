using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Volo.Abp.Domain.Repositories;
using LFG.Prodotti;
using LFG.VarianteProdotti;
using LFG.ImmagineVarianti;
using LFG.Clienti;
using LFG.Recensioni;

namespace LFG.Pages.Vetrina.Prodotto;

[AllowAnonymous]
public class IndexModel : PageModel
{
    private readonly IRepository<LFG.Prodotti.Prodotto, Guid> _prodottoRepo;
    private readonly IRepository<VarianteProdotto, Guid> _varianteRepo;
    private readonly IRepository<ImmagineVariante, Guid> _immagineVarianteRepo;
    private readonly IClientiAppService _clientiAppService;
    private readonly IRepository<Recensione, Guid> _recensioneRepo;

    public IndexModel(
        IRepository<LFG.Prodotti.Prodotto, Guid> prodottoRepo,
        IRepository<VarianteProdotto, Guid> varianteRepo,
        IRepository<ImmagineVariante, Guid> immagineVarianteRepo,
        IClientiAppService clientiAppService,
        IRepository<Recensione, Guid> recensioneRepo)
    {
        _prodottoRepo     = prodottoRepo;
        _varianteRepo     = varianteRepo;
        _immagineVarianteRepo = immagineVarianteRepo;
        _clientiAppService = clientiAppService;
        _recensioneRepo   = recensioneRepo;
    }

    // ── Dati prodotto ────────────────────────────────────────────
    public string Nome       { get; set; } = "";
    public string Prezzo     { get; set; } = "";
    public string Sezione    { get; set; } = "";
    public string? Descrizione { get; set; }
    public bool   PuoAgire   { get; set; }
    public string? SezioneUtente { get; set; }

    public record VarianteVista(
        Guid Id, string Taglia, string Colore, string Materiale,
        int QtaMagazzino, string UrlImmagine, List<string> Immagini);

    public List<VarianteVista> Varianti { get; set; } = new();

    // ── Recensioni ───────────────────────────────────────────────
    public record RecensioneVista(
        int Valutazione, string? Commento, DateTime DataRecensione, bool IsMia);

    public enum StatoRecensione { NonPuoAgire, PuoRecensire, HaGiaRecensito }

    public List<RecensioneVista> Recensioni { get; set; } = new();
    public StatoRecensione Stato { get; set; } = StatoRecensione.NonPuoAgire;

    [BindProperty]
    [Range(RecensioneConsts.ValutazioneMinLength, RecensioneConsts.ValutazioneMaxLength)]
    public int Valutazione { get; set; } = 5;

    [BindProperty]
    [StringLength(RecensioneConsts.CommentoMaxLength)]
    public string? Commento { get; set; }

    public string? MessaggioConferma { get; set; }
    public string? MessaggioErrore   { get; set; }

    // ────────────────────────────────────────────────────────────

    public async Task<IActionResult> OnGetAsync(Guid id)
    {
        var prod = await _prodottoRepo.FindAsync(id);
        if (prod == null)
            return NotFound();

        await CaricaDatiAsync(id, prod);
        return Page();
    }

    public async Task<IActionResult> OnPostRecensioneAsync(Guid id)
    {
        // Guardia server — PRIMA di qualsiasi scrittura
        var prodotto = await _prodottoRepo.GetAsync(id);
        await _clientiAppService.VerificaAccessoSezioneAsync(prodotto.Sezione);

        // Recupera cliente corrente
        var cliente = await _clientiAppService.GetClienteCorrenteAsync();
        if (cliente == null)
        {
            MessaggioErrore = "Cliente non trovato per l'utente corrente.";
            await CaricaDatiAsync(id, prodotto);
            return Page();
        }

        // Controllo anti-doppio (server-side, obbligatorio)
        var esistente = await _recensioneRepo.FirstOrDefaultAsync(
            r => r.ProdottoId == id && r.ClienteId == cliente.Id);

        if (esistente != null)
        {
            MessaggioErrore = "Hai già inviato una recensione per questo prodotto.";
            await CaricaDatiAsync(id, prodotto);
            return Page();
        }

        if (!ModelState.IsValid)
        {
            await CaricaDatiAsync(id, prodotto);
            return Page();
        }

        // Inserimento
        await _recensioneRepo.InsertAsync(
            new Recensione(
                id: Guid.NewGuid(),
                clienteId: cliente.Id,
                prodottoId: id,
                valutazione: Valutazione,
                dataRecensione: DateTime.UtcNow,
                commento: Commento),
            autoSave: true);

        MessaggioConferma = "La tua recensione è stata pubblicata.";
        await CaricaDatiAsync(id, prodotto);
        return Page();
    }

    // ── Helper privato: carica tutti i dati della pagina ────────
    private async Task CaricaDatiAsync(Guid id, LFG.Prodotti.Prodotto prod)
    {
        Nome        = prod.Nome;
        Prezzo      = prod.Prezzo;
        Sezione     = prod.Sezione;
        Descrizione = prod.Descrizione;

        var varianti = await _varianteRepo.GetListAsync(v => v.ProdottoId == id);
        var variantiIds = varianti.Select(v => v.Id).ToList();

        var immaginiVarianti = variantiIds.Any()
            ? await _immagineVarianteRepo.GetListAsync(img => variantiIds.Contains(img.VarianteProdottoId))
            : new List<ImmagineVariante>();

        var immaginiPerVariante = immaginiVarianti
            .GroupBy(img => img.VarianteProdottoId)
            .ToDictionary(g => g.Key, g => g.OrderBy(img => img.Ordine).Select(img => img.Url).ToList());

        Varianti = varianti
            .Select(v =>
            {
                var urlSingola = string.IsNullOrEmpty(v.UrlImmagine) ? "/images/placeholder.png" : v.UrlImmagine;
                var galleria = immaginiPerVariante.TryGetValue(v.Id, out var urls) && urls.Any()
                    ? urls
                    : new List<string> { urlSingola };

                return new VarianteVista(
                    v.Id,
                    v.Taglia    ?? "—",
                    v.Colore    ?? "—",
                    v.Materiale ?? "—",
                    v.QtaMagazzino,
                    urlSingola,
                    galleria);
            })
            .ToList();

        if (User.Identity?.IsAuthenticated == true)
            SezioneUtente = await _clientiAppService.GetSezioneCorrenteAsync();

        PuoAgire = SezioneUtente != null &&
                   string.Equals(SezioneUtente, prod.Sezione, StringComparison.OrdinalIgnoreCase);

        // Recensioni del prodotto
        var tutte = await _recensioneRepo.GetListAsync(r => r.ProdottoId == id);

        Guid? mioClienteId = null;
        if (PuoAgire)
        {
            var cl = await _clientiAppService.GetClienteCorrenteAsync();
            mioClienteId = cl?.Id;
        }

        Recensioni = tutte
            .OrderByDescending(r => r.DataRecensione)
            .Select(r => new RecensioneVista(
                r.Valutazione,
                r.Commento,
                r.DataRecensione,
                mioClienteId.HasValue && r.ClienteId == mioClienteId))
            .ToList();

        // Stato form
        if (!PuoAgire)
            Stato = StatoRecensione.NonPuoAgire;
        else if (mioClienteId.HasValue && tutte.Any(r => r.ClienteId == mioClienteId))
            Stato = StatoRecensione.HaGiaRecensito;
        else
            Stato = StatoRecensione.PuoRecensire;
    }
}
