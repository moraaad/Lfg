using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;
using LFG.Prodotti;
using LFG.VarianteProdotti;
using LFG.Ordini;
using LFG.RigaOrdini;
using LFG.Clienti;
using LFG.Sconti;
using LFG.Helpers;
using LFG.Indirizzi;
using Volo.Abp.Timing;

namespace LFG.Pages.Vetrina.Carrello;

[AllowAnonymous]
public class CheckoutModel : PageModel
{
    private readonly IClientiAppService _clientiAppService;
    private readonly IRepository<Ordine, Guid> _ordineRepo;
    private readonly IRepository<RigaOrdine, Guid> _rigaOrdineRepo;
    private readonly IRepository<VarianteProdotto, Guid> _varianteRepo;
    private readonly IRepository<LFG.Prodotti.Prodotto, Guid> _prodottoRepo;
    private readonly IRepository<Sconto, Guid> _scontoRepo;
    private readonly IRepository<Indirizzo, Guid> _indirizzoRepo;
    private readonly IClock _clock;

    public CheckoutModel(
        IClientiAppService clientiAppService,
        IRepository<Ordine, Guid> ordineRepo,
        IRepository<RigaOrdine, Guid> rigaOrdineRepo,
        IRepository<VarianteProdotto, Guid> varianteRepo,
        IRepository<LFG.Prodotti.Prodotto, Guid> prodottoRepo,
        IRepository<Sconto, Guid> scontoRepo,
        IRepository<Indirizzo, Guid> indirizzoRepo,
        IClock clock)
    {
        _clientiAppService = clientiAppService;
        _ordineRepo         = ordineRepo;
        _rigaOrdineRepo     = rigaOrdineRepo;
        _varianteRepo       = varianteRepo;
        _prodottoRepo       = prodottoRepo;
        _scontoRepo         = scontoRepo;
        _indirizzoRepo      = indirizzoRepo;
        _clock              = clock;
    }

    public bool Autenticato { get; set; }

    public record RigaVista(
        string Nome, string Taglia, string Colore, string UrlImmagine,
        decimal PrezzoUnitario, int Quantita, decimal Subtotale);

    public List<RigaVista> Righe { get; set; } = new();
    public decimal Totale { get; set; }
    public decimal TotaleFinale { get; set; }
    public Sconto? ScontoApplicato { get; set; }
    public string? MessaggioErrore { get; set; }
    public List<string> RigheKO { get; set; } = new();

    public List<Indirizzo> IndirizziCliente { get; set; } = new();

    /// <summary>Id (come stringa) di un Indirizzo esistente scelto, oppure "nuovo".</summary>
    [BindProperty]
    public string IndirizzoOpzione { get; set; } = "";

    [BindProperty]
    [StringLength(IndirizzoConsts.ViaMaxLength)]
    public string? NuovoIndirizzoVia { get; set; }

    [BindProperty]
    public string? NuovoIndirizzoCitta { get; set; }

    [BindProperty]
    [StringLength(IndirizzoConsts.CapMaxLength)]
    public string? NuovoIndirizzoCap { get; set; }

    [BindProperty]
    public string? NuovoIndirizzoProvincia { get; set; }

    [BindProperty]
    public string? NuovoIndirizzoPaese { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "Scegli un metodo di pagamento.")]
    public string MetodoPagamento { get; set; } = "";

    public IReadOnlyList<string> MetodiPagamentoOpzioni => MetodiPagamento.Tutti;

    public async Task<IActionResult> OnGetAsync()
    {
        Autenticato = User.Identity?.IsAuthenticated == true;
        if (!Autenticato)
            return Page();

        var cliente = await _clientiAppService.GetClienteCorrenteAsync();
        if (cliente == null)
            return Page();

        await CaricaRigheAsync(cliente.Id);
        if (!Righe.Any())
            return RedirectToPage("Index");

        await CaricaIndirizziAsync(cliente.Id);
        return Page();
    }

    [UnitOfWork]
    public async Task<IActionResult> OnPostAsync()
    {
        Autenticato = User.Identity?.IsAuthenticated == true;
        var cliente = await _clientiAppService.GetClienteCorrenteAsync();
        if (cliente == null)
        {
            MessaggioErrore = "Devi accedere per confermare l'ordine.";
            return Page();
        }

        var ordine = await _ordineRepo.FirstOrDefaultAsync(o => o.ClienteId == cliente.Id && o.Stato == "Bozza");
        var righe = ordine != null
            ? await _rigaOrdineRepo.GetListAsync(r => r.OrdineId == ordine.Id)
            : new List<RigaOrdine>();

        if (ordine == null || !righe.Any())
        {
            MessaggioErrore = "Il carrello è vuoto.";
            return Page();
        }

        if (!ModelState.IsValid)
        {
            await CaricaRigheAsync(cliente.Id);
            await CaricaIndirizziAsync(cliente.Id);
            return Page();
        }

        if (!MetodiPagamento.Tutti.Contains(MetodoPagamento))
        {
            MessaggioErrore = "Scegli un metodo di pagamento valido.";
            await CaricaRigheAsync(cliente.Id);
            await CaricaIndirizziAsync(cliente.Id);
            return Page();
        }

        // Indirizzo di spedizione: OBBLIGATORIO, o un indirizzo esistente del cliente
        // (anti-manomissione: deve appartenere al cliente corrente) oppure uno nuovo.
        Indirizzo indirizzoScelto;
        if (string.Equals(IndirizzoOpzione, "nuovo", StringComparison.OrdinalIgnoreCase))
        {
            if (string.IsNullOrWhiteSpace(NuovoIndirizzoVia) || string.IsNullOrWhiteSpace(NuovoIndirizzoCap))
            {
                MessaggioErrore = "Inserisci almeno via e CAP per il nuovo indirizzo.";
                await CaricaRigheAsync(cliente.Id);
                await CaricaIndirizziAsync(cliente.Id);
                return Page();
            }

            var nuovoIndirizzo = new Indirizzo(
                Guid.NewGuid(),
                cliente.Id,
                NuovoIndirizzoVia.Trim(),
                NuovoIndirizzoCap.Trim(),
                string.IsNullOrWhiteSpace(NuovoIndirizzoPaese) ? null : NuovoIndirizzoPaese.Trim(),
                string.IsNullOrWhiteSpace(NuovoIndirizzoCitta) ? null : NuovoIndirizzoCitta.Trim(),
                string.IsNullOrWhiteSpace(NuovoIndirizzoProvincia) ? null : NuovoIndirizzoProvincia.Trim());
            await _indirizzoRepo.InsertAsync(nuovoIndirizzo, autoSave: true);
            indirizzoScelto = nuovoIndirizzo;
        }
        else if (Guid.TryParse(IndirizzoOpzione, out var idIndirizzoScelto))
        {
            var indirizzoEsistente = await _indirizzoRepo.FindAsync(idIndirizzoScelto);
            if (indirizzoEsistente == null || indirizzoEsistente.ClienteId != cliente.Id)
            {
                MessaggioErrore = "L'indirizzo selezionato non è valido.";
                await CaricaRigheAsync(cliente.Id);
                await CaricaIndirizziAsync(cliente.Id);
                return Page();
            }

            indirizzoScelto = indirizzoEsistente;
        }
        else
        {
            MessaggioErrore = "Seleziona o inserisci un indirizzo di spedizione.";
            await CaricaRigheAsync(cliente.Id);
            await CaricaIndirizziAsync(cliente.Id);
            return Page();
        }

        var varianteIds = righe.Where(r => r.VarianteProdottoId.HasValue).Select(r => r.VarianteProdottoId!.Value).Distinct().ToList();
        var varianti = (await _varianteRepo.GetListAsync(v => varianteIds.Contains(v.Id))).ToDictionary(v => v.Id);

        // Ricontrollo scorte: se anche una riga è insufficiente, non si scala nulla
        var insufficienti = righe
            .Where(r => !r.VarianteProdottoId.HasValue
                        || !varianti.TryGetValue(r.VarianteProdottoId.Value, out var v)
                        || v.QtaMagazzino < r.Quantita)
            .ToList();

        if (insufficienti.Any())
        {
            var prodottoIds = insufficienti
                .Where(r => r.VarianteProdottoId.HasValue && varianti.ContainsKey(r.VarianteProdottoId.Value))
                .Select(r => varianti[r.VarianteProdottoId!.Value].ProdottoId)
                .Distinct().ToList();
            var prodottiKo = (await _prodottoRepo.GetListAsync(p => prodottoIds.Contains(p.Id))).ToDictionary(p => p.Id);

            RigheKO = insufficienti
                .Select(r =>
                {
                    if (!r.VarianteProdottoId.HasValue || !varianti.TryGetValue(r.VarianteProdottoId.Value, out var v))
                        return "Una variante non è più disponibile.";
                    var nome = prodottiKo.TryGetValue(v.ProdottoId, out var p) ? p.Nome : "—";
                    return $"{nome} ({v.Taglia} · {v.Colore}): richiesti {r.Quantita}, disponibili {v.QtaMagazzino}.";
                })
                .ToList();

            MessaggioErrore = "Scorta insufficiente per uno o più articoli. Nessun articolo è stato scalato.";
            await CaricaRigheAsync(cliente.Id);
            await CaricaIndirizziAsync(cliente.Id);
            return Page();
        }

        // Tutte le righe disponibili: scala il magazzino e conferma l'ordine
        foreach (var riga in righe)
        {
            var variante = varianti[riga.VarianteProdottoId!.Value];
            variante.QtaMagazzino -= riga.Quantita;
            await _varianteRepo.UpdateAsync(variante);
        }

        // Ri-valida lo sconto (potrebbe essere scaduto o aver raggiunto il limite
        // tra l'applicazione nel carrello e la conferma): se non è più valido lo si
        // rimuove e si ricalcola il totale pieno, avvisando l'utente dopo il redirect.
        var totaleFinale = righe.Sum(r => r.PrezzoUnitario * r.Quantita);
        if (ordine.ScontoId.HasValue)
        {
            var sconto = await _scontoRepo.FindAsync(ordine.ScontoId.Value);
            var (valido, _) = sconto != null
                ? await ScontoHelper.ValidaAsync(sconto, cliente.Sezione, _clock.Now, _ordineRepo)
                : (false, null);

            if (valido && sconto != null)
            {
                totaleFinale = ScontoHelper.Calcola(sconto, totaleFinale);
            }
            else
            {
                ordine.ScontoId = null;
                TempData["AvvisoSconto"] = "Il codice sconto applicato non è più valido: il totale è stato ricalcolato senza sconto.";
            }
        }

        ordine.Stato = "Confermato";
        ordine.IndirizzoId = indirizzoScelto.Id;
        // Oltre alla FK strutturata, si salva anche una copia testuale per le viste
        // (es. griglia admin) che leggono ancora IndSpedizione senza fare il join.
        ordine.IndSpedizione = FormattaIndirizzo(indirizzoScelto);
        ordine.MetodoPagamento = MetodoPagamento;
        ordine.ImportoTotale = totaleFinale;
        ordine.DataOrdine = _clock.Now;
        await _ordineRepo.UpdateAsync(ordine);

        return RedirectToPage("Confermato", new { id = ordine.Id });
    }

    private async Task CaricaRigheAsync(Guid clienteId)
    {
        var ordine = await _ordineRepo.FirstOrDefaultAsync(o => o.ClienteId == clienteId && o.Stato == "Bozza");
        if (ordine == null)
            return;

        var righe = await _rigaOrdineRepo.GetListAsync(r => r.OrdineId == ordine.Id);
        if (!righe.Any())
            return;

        var varianteIds = righe.Where(r => r.VarianteProdottoId.HasValue).Select(r => r.VarianteProdottoId!.Value).ToList();
        var varianti = (await _varianteRepo.GetListAsync(v => varianteIds.Contains(v.Id))).ToDictionary(v => v.Id);

        var prodottoIds = varianti.Values.Select(v => v.ProdottoId).Distinct().ToList();
        var prodotti = (await _prodottoRepo.GetListAsync(p => prodottoIds.Contains(p.Id))).ToDictionary(p => p.Id);

        Righe = righe
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

        Totale = Righe.Sum(r => r.Subtotale);
        TotaleFinale = Totale;

        if (ordine.ScontoId.HasValue)
        {
            var sconto = await _scontoRepo.FindAsync(ordine.ScontoId.Value);
            if (sconto != null)
            {
                ScontoApplicato = sconto;
                TotaleFinale = ScontoHelper.Calcola(sconto, Totale);
            }
        }
    }

    private async Task CaricaIndirizziAsync(Guid clienteId)
    {
        IndirizziCliente = await _indirizzoRepo.GetListAsync(i => i.ClienteId == clienteId);
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

        // Il campo legacy IndSpedizione è limitato in DB: tronca per sicurezza.
        return testo.Length > OrdineConsts.IndSpedizioneMaxLength
            ? testo[..OrdineConsts.IndSpedizioneMaxLength]
            : testo;
    }
}
