using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Data;

namespace LFG.ImmagineVarianti;

public abstract class ImmagineVarianteManagerBase : DomainService
{
    protected IImmagineVarianteRepository _immagineVarianteRepository;

    public ImmagineVarianteManagerBase(IImmagineVarianteRepository immagineVarianteRepository)
    {
        _immagineVarianteRepository = immagineVarianteRepository;
    }

    public virtual async Task<ImmagineVariante> CreateAsync(Guid varianteProdottoId, string url, int ordine)
    {
        Check.NotNullOrWhiteSpace(url, nameof(url));
        Check.Length(url, nameof(url), ImmagineVarianteConsts.UrlMaxLength);
        var immagineVariante = new ImmagineVariante(GuidGenerator.Create(), varianteProdottoId, url, ordine);
        return await _immagineVarianteRepository.InsertAsync(immagineVariante);
    }

    public virtual async Task<ImmagineVariante> UpdateAsync(Guid id, Guid varianteProdottoId, string url, int ordine, [CanBeNull] string? concurrencyStamp = null)
    {
        Check.NotNullOrWhiteSpace(url, nameof(url));
        Check.Length(url, nameof(url), ImmagineVarianteConsts.UrlMaxLength);
        var immagineVariante = await _immagineVarianteRepository.GetAsync(id);
        immagineVariante.VarianteProdottoId = varianteProdottoId;
        immagineVariante.Url = url;
        immagineVariante.Ordine = ordine;
        immagineVariante.SetConcurrencyStampIfNotNull(concurrencyStamp);
        return await _immagineVarianteRepository.UpdateAsync(immagineVariante);
    }
}