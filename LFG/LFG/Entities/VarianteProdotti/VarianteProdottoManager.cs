using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace LFG.VarianteProdotti;

public abstract class VarianteProdottoManagerBase : DomainService
{
    protected IVarianteProdottoRepository _varianteProdottoRepository;

    public VarianteProdottoManagerBase(IVarianteProdottoRepository varianteProdottoRepository)
    {
        _varianteProdottoRepository = varianteProdottoRepository;
    }

    public virtual async Task<VarianteProdotto> CreateAsync(Guid prodottoId, int qtaMagazzino, string? taglia = null, string? colore = null, string? materiale = null, string? urlImmagine = null)
    {
        Check.Length(urlImmagine, nameof(urlImmagine), VarianteProdottoConsts.UrlImmagineMaxLength);
        var varianteProdotto = new VarianteProdotto(GuidGenerator.Create(), prodottoId, qtaMagazzino, taglia, colore, materiale, urlImmagine);
        return await _varianteProdottoRepository.InsertAsync(varianteProdotto);
    }

    public virtual async Task<VarianteProdotto> UpdateAsync(Guid id, Guid prodottoId, int qtaMagazzino, string? taglia = null, string? colore = null, string? materiale = null, string? urlImmagine = null)
    {
        Check.Length(urlImmagine, nameof(urlImmagine), VarianteProdottoConsts.UrlImmagineMaxLength);
        var varianteProdotto = await _varianteProdottoRepository.GetAsync(id);
        varianteProdotto.ProdottoId = prodottoId;
        varianteProdotto.QtaMagazzino = qtaMagazzino;
        varianteProdotto.Taglia = taglia;
        varianteProdotto.Colore = colore;
        varianteProdotto.Materiale = materiale;
        varianteProdotto.UrlImmagine = urlImmagine;
        return await _varianteProdottoRepository.UpdateAsync(varianteProdotto);
    }
}