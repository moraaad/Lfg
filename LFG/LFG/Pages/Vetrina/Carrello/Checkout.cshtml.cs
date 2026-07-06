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

namespace LFG.Pages.Vetrina.Carrello;

[AllowAnonymous]
public class CheckoutModel : PageModel
{
    private readonly IClientiAppService _clientiAppService;
    private readonly IRepository<Ordine, Guid> _ordineRepo;
    private readonly IRepository<RigaOrdine, Guid> _rigaOrdineRepo;
    private readonly IRepository<VarianteProdotto, Guid> _varianteRepo;
    private readonly IRepository<LFG.Prodotti.Prodotto, Guid> _prodottoRepo;

    public CheckoutModel(
        IClientiAppService clientiAppService,
        IRepository<Ordine, Guid> ordineRepo,
        IRepository<RigaOrdine, Guid> rigaOrdineRepo,
        IRepository<VarianteProdotto, Guid> varianteRepo,
        IRepository<LFG.Prodotti.Prodotto, Guid> prodottoRepo)
    {
        _clientiAppService = clientiAppService;
        _ordineRepo         = ordineRepo;
        _rigaOrdineRepo     = rigaOrdineRepo;
        _varianteRepo       = varianteRepo;
        _prodottoRepo       = prodottoRepo;
    }

    public bool Autenticato { get; set; }

    public record RigaVista(
        string Nome, string Taglia, string Colore, string UrlImmagine,
        decimal PrezzoUnitario, int Quantita, decimal Subtotale);

    public List<RigaVista> Righe { get; set; } = new();
    public decimal Totale { get; set; }
    public string? MessaggioErrore { get; set; }
    public List<string> RigheKO { get; set; } = new();

    [BindProperty]
    [Required(ErrorMessage = "Indica un indirizzo di spedizione.")]
    [StringLength(OrdineConsts.IndSpedizioneMaxLength)]
    public string IndSpedizione { get; set; } = "";

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
            return Page();
        }

        if (!MetodiPagamento.Tutti.Contains(MetodoPagamento))
        {
            MessaggioErrore = "Scegli un metodo di pagamento valido.";
            await CaricaRigheAsync(cliente.Id);
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
            return Page();
        }

        // Tutte le righe disponibili: scala il magazzino e conferma l'ordine
        foreach (var riga in righe)
        {
            var variante = varianti[riga.VarianteProdottoId!.Value];
            variante.QtaMagazzino -= riga.Quantita;
            await _varianteRepo.UpdateAsync(variante);
        }

        ordine.Stato = "Confermato";
        ordine.IndSpedizione = IndSpedizione;
        ordine.MetodoPagamento = MetodoPagamento;
        ordine.ImportoTotale = righe.Sum(r => r.PrezzoUnitario * r.Quantita);
        ordine.DataOrdine = DateTime.UtcNow;
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
    }
}
