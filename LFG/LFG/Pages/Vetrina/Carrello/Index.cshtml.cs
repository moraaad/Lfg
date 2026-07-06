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
using LFG.Ordini;
using LFG.RigaOrdini;
using LFG.Clienti;
using LFG.Sconti;
using LFG.Helpers;

namespace LFG.Pages.Vetrina.Carrello;

[AllowAnonymous]
public class IndexModel : PageModel
{
    private readonly IClientiAppService _clientiAppService;
    private readonly IRepository<Ordine, Guid> _ordineRepo;
    private readonly IRepository<RigaOrdine, Guid> _rigaOrdineRepo;
    private readonly IRepository<VarianteProdotto, Guid> _varianteRepo;
    private readonly IRepository<LFG.Prodotti.Prodotto, Guid> _prodottoRepo;
    private readonly IRepository<Sconto, Guid> _scontoRepo;

    public IndexModel(
        IClientiAppService clientiAppService,
        IRepository<Ordine, Guid> ordineRepo,
        IRepository<RigaOrdine, Guid> rigaOrdineRepo,
        IRepository<VarianteProdotto, Guid> varianteRepo,
        IRepository<LFG.Prodotti.Prodotto, Guid> prodottoRepo,
        IRepository<Sconto, Guid> scontoRepo)
    {
        _clientiAppService = clientiAppService;
        _ordineRepo         = ordineRepo;
        _rigaOrdineRepo     = rigaOrdineRepo;
        _varianteRepo       = varianteRepo;
        _prodottoRepo       = prodottoRepo;
        _scontoRepo         = scontoRepo;
    }

    public bool Autenticato { get; set; }

    public record RigaVista(
        Guid RigaId, Guid ProdottoId, string Nome, string Taglia, string Colore,
        string UrlImmagine, decimal PrezzoUnitario, int Quantita, decimal Subtotale);

    public List<RigaVista> Righe { get; set; } = new();
    public decimal Totale { get; set; }
    public decimal TotaleFinale { get; set; }
    public Sconto? ScontoApplicato { get; set; }
    public string? MessaggioErrore { get; set; }
    public string? MessaggioConferma { get; set; }

    [BindProperty]
    public string CodiceSconto { get; set; } = "";

    public async Task<IActionResult> OnGetAsync()
    {
        Autenticato = User.Identity?.IsAuthenticated == true;
        if (!Autenticato)
            return Page();

        var cliente = await _clientiAppService.GetClienteCorrenteAsync();
        if (cliente == null)
            return Page();

        await CaricaCarrelloAsync(cliente.Id);
        return Page();
    }

    public async Task<IActionResult> OnPostAggiornaQuantitaAsync(Guid rigaId, int quantita)
    {
        var cliente = await _clientiAppService.GetClienteCorrenteAsync();
        if (cliente == null)
            return RedirectToPage();

        var riga = await _rigaOrdineRepo.FindAsync(rigaId);
        if (riga != null && await AppartieneABozzaDelClienteAsync(riga, cliente.Id))
        {
            if (quantita <= 0)
            {
                await _rigaOrdineRepo.DeleteAsync(rigaId, autoSave: true);
            }
            else
            {
                riga.Quantita = Math.Min(quantita, RigaOrdineConsts.QuantitaMaxLength);
                await _rigaOrdineRepo.UpdateAsync(riga, autoSave: true);
            }
        }

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostRimuoviAsync(Guid rigaId)
    {
        var cliente = await _clientiAppService.GetClienteCorrenteAsync();
        if (cliente == null)
            return RedirectToPage();

        var riga = await _rigaOrdineRepo.FindAsync(rigaId);
        if (riga != null && await AppartieneABozzaDelClienteAsync(riga, cliente.Id))
            await _rigaOrdineRepo.DeleteAsync(rigaId, autoSave: true);

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostSvuotaAsync()
    {
        var cliente = await _clientiAppService.GetClienteCorrenteAsync();
        if (cliente == null)
            return RedirectToPage();

        var ordine = await _ordineRepo.FirstOrDefaultAsync(o => o.ClienteId == cliente.Id && o.Stato == "Bozza");
        if (ordine != null)
        {
            var righe = await _rigaOrdineRepo.GetListAsync(r => r.OrdineId == ordine.Id);
            if (righe.Any())
                await _rigaOrdineRepo.DeleteManyAsync(righe.Select(r => r.Id).ToList(), autoSave: true);
        }

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostApplicaScontoAsync()
    {
        Autenticato = User.Identity?.IsAuthenticated == true;
        var cliente = await _clientiAppService.GetClienteCorrenteAsync();
        if (cliente == null)
            return RedirectToPage();

        var ordine = await _ordineRepo.FirstOrDefaultAsync(o => o.ClienteId == cliente.Id && o.Stato == "Bozza");
        if (ordine == null)
        {
            MessaggioErrore = "Il carrello è vuoto.";
            await CaricaCarrelloAsync(cliente.Id);
            return Page();
        }

        var codice = (CodiceSconto ?? "").Trim();
        var sconto = await _scontoRepo.FirstOrDefaultAsync(s => s.Codice == codice);
        if (sconto == null)
        {
            MessaggioErrore = "Codice sconto non valido.";
            await CaricaCarrelloAsync(cliente.Id);
            return Page();
        }

        var (valido, errore) = await ScontoHelper.ValidaAsync(sconto, cliente.Sezione, DateTime.UtcNow, _ordineRepo);
        if (!valido)
        {
            MessaggioErrore = errore;
            await CaricaCarrelloAsync(cliente.Id);
            return Page();
        }

        // Un solo sconto per ordine: quello nuovo sostituisce l'eventuale precedente
        ordine.ScontoId = sconto.Id;
        await _ordineRepo.UpdateAsync(ordine, autoSave: true);

        MessaggioConferma = $"Codice sconto \"{sconto.Codice}\" applicato.";
        await CaricaCarrelloAsync(cliente.Id);
        return Page();
    }

    public async Task<IActionResult> OnPostRimuoviScontoAsync()
    {
        var cliente = await _clientiAppService.GetClienteCorrenteAsync();
        if (cliente == null)
            return RedirectToPage();

        var ordine = await _ordineRepo.FirstOrDefaultAsync(o => o.ClienteId == cliente.Id && o.Stato == "Bozza");
        if (ordine != null && ordine.ScontoId.HasValue)
        {
            ordine.ScontoId = null;
            await _ordineRepo.UpdateAsync(ordine, autoSave: true);
        }

        return RedirectToPage();
    }

    private async Task<bool> AppartieneABozzaDelClienteAsync(RigaOrdine riga, Guid clienteId)
    {
        if (!riga.OrdineId.HasValue)
            return false;

        var ordine = await _ordineRepo.FindAsync(riga.OrdineId.Value);
        return ordine != null && ordine.ClienteId == clienteId && ordine.Stato == "Bozza";
    }

    private async Task CaricaCarrelloAsync(Guid clienteId)
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
                    r.Id,
                    variante.ProdottoId,
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
}
