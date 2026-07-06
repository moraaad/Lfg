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
using LFG.Clienti;
using LFG.ListeDesideri;
using LFG.ElementoListe;

namespace LFG.Pages.Vetrina.Preferiti;

[AllowAnonymous]
public class IndexModel : PageModel
{
    private readonly IClientiAppService _clientiAppService;
    private readonly IRepository<ListaDesideri, Guid> _listaDesideriRepo;
    private readonly IRepository<ElementoLista, Guid> _elementoListaRepo;
    private readonly IRepository<VarianteProdotto, Guid> _varianteRepo;
    private readonly IRepository<LFG.Prodotti.Prodotto, Guid> _prodottoRepo;

    public IndexModel(
        IClientiAppService clientiAppService,
        IRepository<ListaDesideri, Guid> listaDesideriRepo,
        IRepository<ElementoLista, Guid> elementoListaRepo,
        IRepository<VarianteProdotto, Guid> varianteRepo,
        IRepository<LFG.Prodotti.Prodotto, Guid> prodottoRepo)
    {
        _clientiAppService = clientiAppService;
        _listaDesideriRepo = listaDesideriRepo;
        _elementoListaRepo = elementoListaRepo;
        _varianteRepo      = varianteRepo;
        _prodottoRepo      = prodottoRepo;
    }

    public bool Autenticato { get; set; }
    public string? SezioneUtente { get; set; }

    public record PreferitoVista(Guid ElementoListaId, Guid ProdottoId, string Nome, string Prezzo, string UrlImmagine);

    public List<PreferitoVista> Preferiti { get; set; } = new();

    public async Task<IActionResult> OnGetAsync()
    {
        Autenticato = User.Identity?.IsAuthenticated == true;
        if (!Autenticato)
            return Page();

        var cliente = await _clientiAppService.GetClienteCorrenteAsync();
        if (cliente == null)
            return Page();

        SezioneUtente = cliente.Sezione;
        await CaricaPreferitiAsync(cliente.Id);
        return Page();
    }

    public async Task<IActionResult> OnPostRimuoviAsync(Guid elementoListaId)
    {
        var cliente = await _clientiAppService.GetClienteCorrenteAsync();
        if (cliente == null)
            return RedirectToPage();

        var elemento = await _elementoListaRepo.FindAsync(elementoListaId);
        if (elemento != null)
        {
            var lista = elemento.ListaDesideriId.HasValue
                ? await _listaDesideriRepo.FindAsync(elemento.ListaDesideriId.Value)
                : null;

            // Verifica che l'elemento appartenga alla wishlist del cliente corrente
            if (lista != null && lista.ClienteId == cliente.Id)
            {
                await _elementoListaRepo.DeleteAsync(elementoListaId, autoSave: true);
            }
        }

        return RedirectToPage();
    }

    private async Task CaricaPreferitiAsync(Guid clienteId)
    {
        var lista = await _listaDesideriRepo.FirstOrDefaultAsync(l => l.ClienteId == clienteId);
        if (lista == null)
            return;

        var elementi = await _elementoListaRepo.GetListAsync(e => e.ListaDesideriId == lista.Id);
        if (!elementi.Any())
            return;

        var varianteIds = elementi.Select(e => e.VarianteProdottoId).ToList();
        var varianti = (await _varianteRepo.GetListAsync(v => varianteIds.Contains(v.Id)))
            .ToDictionary(v => v.Id);

        var prodottoIds = varianti.Values.Select(v => v.ProdottoId).Distinct().ToList();
        var prodotti = (await _prodottoRepo.GetListAsync(p => prodottoIds.Contains(p.Id)))
            .ToDictionary(p => p.Id);

        Preferiti = elementi
            .Where(e => varianti.ContainsKey(e.VarianteProdottoId))
            .Select(e =>
            {
                var variante = varianti[e.VarianteProdottoId];
                return prodotti.TryGetValue(variante.ProdottoId, out var prod)
                    ? new PreferitoVista(
                        e.Id,
                        prod.Id,
                        prod.Nome,
                        prod.Prezzo,
                        string.IsNullOrEmpty(variante.UrlImmagine) ? "/images/placeholder.png" : variante.UrlImmagine)
                    : null;
            })
            .Where(p => p != null)
            .Select(p => p!)
            .ToList();
    }
}
