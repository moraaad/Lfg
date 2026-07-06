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

namespace LFG.Pages.Vetrina.Carrello;

[AllowAnonymous]
public class ConfermatoModel : PageModel
{
    private readonly IClientiAppService _clientiAppService;
    private readonly IRepository<Ordine, Guid> _ordineRepo;
    private readonly IRepository<RigaOrdine, Guid> _rigaOrdineRepo;
    private readonly IRepository<VarianteProdotto, Guid> _varianteRepo;
    private readonly IRepository<LFG.Prodotti.Prodotto, Guid> _prodottoRepo;

    public ConfermatoModel(
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
    public bool Trovato { get; set; }
    public string? IndSpedizione { get; set; }
    public string? MetodoPagamento { get; set; }
    public DateTime DataOrdine { get; set; }
    public decimal Totale { get; set; }

    public record RigaVista(
        string Nome, string Taglia, string Colore, string UrlImmagine,
        decimal PrezzoUnitario, int Quantita, decimal Subtotale);

    public List<RigaVista> Righe { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(Guid id)
    {
        Autenticato = User.Identity?.IsAuthenticated == true;
        if (!Autenticato)
            return Page();

        var cliente = await _clientiAppService.GetClienteCorrenteAsync();
        if (cliente == null)
            return Page();

        var ordine = await _ordineRepo.FindAsync(id);
        if (ordine == null || ordine.ClienteId != cliente.Id || ordine.Stato != "Confermato")
            return Page();

        Trovato = true;
        IndSpedizione = ordine.IndSpedizione;
        MetodoPagamento = ordine.MetodoPagamento;
        DataOrdine = ordine.DataOrdine;
        Totale = ordine.ImportoTotale;

        var righe = await _rigaOrdineRepo.GetListAsync(r => r.OrdineId == ordine.Id);
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

        return Page();
    }
}
