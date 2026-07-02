using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using LFG.ImmagineVarianti;
using LFG.Permissions;
using LFG.Shared;

namespace LFG.Web.Pages.VarianteProdotti;

[Authorize(LFGPermissions.ImmagineVarianti.Default)]
public class ImmaginiModalModel : AbpPageModel
{
    private const int MaxFilePerCaricamento = 5;

    private readonly IImmagineVariantiAppService _immagineVariantiAppService;
    private readonly ImmagineVarianteFileStorage _fileStorage;

    [BindProperty(SupportsGet = true)]
    public Guid VarianteProdottoId { get; set; }

    public List<ImmagineVarianteDto> Immagini { get; set; } = new();

    public ImmaginiModalModel(IImmagineVariantiAppService immagineVariantiAppService, ImmagineVarianteFileStorage fileStorage)
    {
        _immagineVariantiAppService = immagineVariantiAppService;
        _fileStorage = fileStorage;
    }

    public async Task OnGetAsync()
    {
        await CaricaImmaginiAsync();
    }

    public async Task<IActionResult> OnPostUploadAsync(List<IFormFile> files)
    {
        files = (files ?? new List<IFormFile>()).Where(f => f.Length > 0).ToList();

        if (files.Count == 0)
        {
            throw new UserFriendlyException("Seleziona almeno un file da caricare.");
        }

        if (files.Count > MaxFilePerCaricamento)
        {
            throw new UserFriendlyException($"Puoi caricare al massimo {MaxFilePerCaricamento} immagini alla volta.");
        }

        foreach (var file in files)
        {
            var errore = await _fileStorage.ValidaAsync(file);
            if (errore != null)
            {
                throw new UserFriendlyException(errore);
            }
        }

        await CaricaImmaginiAsync();
        var prossimoOrdine = Immagini.Any() ? Immagini.Max(i => i.Ordine) + 1 : 0;

        foreach (var file in files)
        {
            var url = await _fileStorage.SalvaAsync(file);
            await _immagineVariantiAppService.CreateAsync(new ImmagineVarianteCreateDto
            {
                VarianteProdottoId = VarianteProdottoId,
                Url = url,
                Ordine = prossimoOrdine,
            });
            prossimoOrdine++;
        }

        await CaricaImmaginiAsync();
        return new JsonResult(new { success = true, immagini = ProiettaPerClient() });
    }

    public async Task<IActionResult> OnPostEliminaAsync(Guid id)
    {
        var immagine = await _immagineVariantiAppService.GetAsync(id);
        await _immagineVariantiAppService.DeleteAsync(id);
        _fileStorage.Elimina(immagine.Url);

        await CaricaImmaginiAsync();
        return new JsonResult(new { success = true, immagini = ProiettaPerClient() });
    }

    public async Task<IActionResult> OnPostSpostaAsync(Guid id, bool su)
    {
        await CaricaImmaginiAsync();

        var indice = Immagini.FindIndex(i => i.Id == id);
        if (indice < 0)
        {
            throw new UserFriendlyException("Immagine non trovata.");
        }

        var indiceVicino = su ? indice - 1 : indice + 1;
        if (indiceVicino >= 0 && indiceVicino < Immagini.Count)
        {
            var corrente = Immagini[indice];
            var vicino = Immagini[indiceVicino];

            var ordineCorrente = corrente.Ordine;
            var ordineVicino = vicino.Ordine;

            await _immagineVariantiAppService.UpdateAsync(corrente.Id, new ImmagineVarianteUpdateDto
            {
                VarianteProdottoId = corrente.VarianteProdottoId,
                Url = corrente.Url,
                Ordine = ordineVicino,
                ConcurrencyStamp = corrente.ConcurrencyStamp,
            });

            await _immagineVariantiAppService.UpdateAsync(vicino.Id, new ImmagineVarianteUpdateDto
            {
                VarianteProdottoId = vicino.VarianteProdottoId,
                Url = vicino.Url,
                Ordine = ordineCorrente,
                ConcurrencyStamp = vicino.ConcurrencyStamp,
            });

            await CaricaImmaginiAsync();
        }

        return new JsonResult(new { success = true, immagini = ProiettaPerClient() });
    }

    private async Task CaricaImmaginiAsync()
    {
        var risultato = await _immagineVariantiAppService.GetListAsync(new GetImmagineVariantiInput
        {
            VarianteProdottoId = VarianteProdottoId,
            Sorting = "ImmagineVariante.Ordine asc",
            MaxResultCount = 100,
        });

        Immagini = risultato.Items
            .Select(x => x.ImmagineVariante)
            .OrderBy(x => x.Ordine)
            .ToList();
    }

    private object ProiettaPerClient()
    {
        return Immagini.Select(i => new { id = i.Id, url = i.Url, ordine = i.Ordine }).ToList();
    }
}
