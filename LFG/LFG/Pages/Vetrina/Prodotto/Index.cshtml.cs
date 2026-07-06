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
using LFG.ListeDesideri;
using LFG.ElementoListe;
using LFG.Ordini;
using LFG.RigaOrdini;
using LFG.Helpers;

namespace LFG.Pages.Vetrina.Prodotto;

[AllowAnonymous]
public class IndexModel : PageModel
{
    private readonly IRepository<LFG.Prodotti.Prodotto, Guid> _prodottoRepo;
    private readonly IRepository<VarianteProdotto, Guid> _varianteRepo;
    private readonly IRepository<ImmagineVariante, Guid> _immagineVarianteRepo;
    private readonly IClientiAppService _clientiAppService;
    private readonly IRepository<Recensione, Guid> _recensioneRepo;
    private readonly IRepository<ListaDesideri, Guid> _listaDesideriRepo;
    private readonly IRepository<ElementoLista, Guid> _elementoListaRepo;
    private readonly IRepository<Ordine, Guid> _ordineRepo;
    private readonly IRepository<RigaOrdine, Guid> _rigaOrdineRepo;

    public IndexModel(
        IRepository<LFG.Prodotti.Prodotto, Guid> prodottoRepo,
        IRepository<VarianteProdotto, Guid> varianteRepo,
        IRepository<ImmagineVariante, Guid> immagineVarianteRepo,
        IClientiAppService clientiAppService,
        IRepository<Recensione, Guid> recensioneRepo,
        IRepository<ListaDesideri, Guid> listaDesideriRepo,
        IRepository<ElementoLista, Guid> elementoListaRepo,
        IRepository<Ordine, Guid> ordineRepo,
        IRepository<RigaOrdine, Guid> rigaOrdineRepo)
    {
        _prodottoRepo     = prodottoRepo;
        _varianteRepo     = varianteRepo;
        _immagineVarianteRepo = immagineVarianteRepo;
        _clientiAppService = clientiAppService;
        _recensioneRepo   = recensioneRepo;
        _listaDesideriRepo = listaDesideriRepo;
        _elementoListaRepo = elementoListaRepo;
        _ordineRepo       = ordineRepo;
        _rigaOrdineRepo   = rigaOrdineRepo;
    }

    // ── Dati prodotto ────────────────────────────────────────────
    public string Nome       { get; set; } = "";
    public string Prezzo     { get; set; } = "";
    public string Sezione    { get; set; } = "";
    public string? Descrizione { get; set; }
    public bool   PuoAgire   { get; set; }
    public string? SezioneUtente { get; set; }
    public bool   InWishlist { get; set; }
    public bool   CarrelloAggiornato { get; set; }

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

    [BindProperty]
    public Guid VarianteId { get; set; }

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

    public async Task<IActionResult> OnPostWishlistToggleAsync(Guid id)
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

        var variantiProdotto = await _varianteRepo.GetListAsync(v => v.ProdottoId == id);
        var variantiIds = variantiProdotto.Select(v => v.Id).ToList();

        if (!variantiIds.Any())
        {
            MessaggioErrore = "Questo prodotto non ha varianti disponibili.";
            await CaricaDatiAsync(id, prodotto);
            return Page();
        }

        // Get-or-create della wishlist del cliente (una sola lista, automatica)
        var lista = await _listaDesideriRepo.FirstOrDefaultAsync(l => l.ClienteId == cliente.Id);
        if (lista == null)
        {
            lista = new ListaDesideri(Guid.NewGuid(), cliente.Id, DateTime.UtcNow, "Preferiti");
            await _listaDesideriRepo.InsertAsync(lista, autoSave: true);
        }

        // Il prodotto è già in lista? (anti-doppio, server-side, obbligatorio)
        var elementiEsistenti = await _elementoListaRepo.GetListAsync(
            e => e.ListaDesideriId == lista.Id && variantiIds.Contains(e.VarianteProdottoId));

        if (elementiEsistenti.Any())
        {
            // Toggle: il prodotto era già nei preferiti -> lo rimuove
            await _elementoListaRepo.DeleteManyAsync(elementiEsistenti.Select(e => e.Id).ToList(), autoSave: true);
            MessaggioConferma = "Prodotto rimosso dai preferiti.";
        }
        else
        {
            var varianteId = variantiProdotto.OrderBy(v => v.CreationTime).First().Id;
            await _elementoListaRepo.InsertAsync(
                new ElementoLista(Guid.NewGuid(), varianteId, lista.Id, DateTime.UtcNow),
                autoSave: true);
            MessaggioConferma = "Prodotto aggiunto ai preferiti.";
        }

        await CaricaDatiAsync(id, prodotto);
        return Page();
    }

    public async Task<IActionResult> OnPostCarrelloAggiungiAsync(Guid id)
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

        var variante = await _varianteRepo.FindAsync(VarianteId);
        if (variante == null || variante.ProdottoId != id)
        {
            MessaggioErrore = "Variante non valida.";
            await CaricaDatiAsync(id, prodotto);
            return Page();
        }

        // Get-or-create dell'ordine bozza del cliente (un solo carrello attivo)
        var ordine = await _ordineRepo.FirstOrDefaultAsync(
            o => o.ClienteId == cliente.Id && o.Stato == "Bozza");
        if (ordine == null)
        {
            ordine = new Ordine(Guid.NewGuid(), cliente.Id, null, DateTime.UtcNow, 0m, "Bozza");
            await _ordineRepo.InsertAsync(ordine, autoSave: true);
        }

        // Riga già presente per questa variante nella bozza? Incrementa, altrimenti crea
        var rigaEsistente = await _rigaOrdineRepo.FirstOrDefaultAsync(
            r => r.OrdineId == ordine.Id && r.VarianteProdottoId == variante.Id);

        if (rigaEsistente != null)
        {
            rigaEsistente.Quantita += 1;
            await _rigaOrdineRepo.UpdateAsync(rigaEsistente, autoSave: true);
        }
        else
        {
            await _rigaOrdineRepo.InsertAsync(
                new RigaOrdine(Guid.NewGuid(), ordine.Id, variante.Id, 1, PrezzoHelper.Parse(prodotto.Prezzo)),
                autoSave: true);
        }

        MessaggioConferma = "Prodotto aggiunto al carrello.";
        CarrelloAggiornato = true;
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

            if (mioClienteId.HasValue && variantiIds.Any())
            {
                var lista = await _listaDesideriRepo.FirstOrDefaultAsync(l => l.ClienteId == mioClienteId);
                if (lista != null)
                {
                    InWishlist = await _elementoListaRepo.AnyAsync(
                        e => e.ListaDesideriId == lista.Id && variantiIds.Contains(e.VarianteProdottoId));
                }
            }
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
