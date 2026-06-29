using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Data;

namespace LFG.RigaOrdini;

public abstract class RigaOrdineManagerBase : DomainService
{
    protected IRigaOrdineRepository _rigaOrdineRepository;

    public RigaOrdineManagerBase(IRigaOrdineRepository rigaOrdineRepository)
    {
        _rigaOrdineRepository = rigaOrdineRepository;
    }

    public virtual async Task<RigaOrdine> CreateAsync(Guid? ordineId, Guid? varianteProdottoId, int quantita, decimal prezzoUnitario)
    {
        Check.Range(quantita, nameof(quantita), RigaOrdineConsts.QuantitaMinLength, RigaOrdineConsts.QuantitaMaxLength);
        Check.Range(prezzoUnitario, nameof(prezzoUnitario), RigaOrdineConsts.PrezzoUnitarioMinLength, RigaOrdineConsts.PrezzoUnitarioMaxLength);
        var rigaOrdine = new RigaOrdine(GuidGenerator.Create(), ordineId, varianteProdottoId, quantita, prezzoUnitario);
        return await _rigaOrdineRepository.InsertAsync(rigaOrdine);
    }

    public virtual async Task<RigaOrdine> UpdateAsync(Guid id, Guid? ordineId, Guid? varianteProdottoId, int quantita, decimal prezzoUnitario, [CanBeNull] string? concurrencyStamp = null)
    {
        Check.Range(quantita, nameof(quantita), RigaOrdineConsts.QuantitaMinLength, RigaOrdineConsts.QuantitaMaxLength);
        Check.Range(prezzoUnitario, nameof(prezzoUnitario), RigaOrdineConsts.PrezzoUnitarioMinLength, RigaOrdineConsts.PrezzoUnitarioMaxLength);
        var rigaOrdine = await _rigaOrdineRepository.GetAsync(id);
        rigaOrdine.OrdineId = ordineId;
        rigaOrdine.VarianteProdottoId = varianteProdottoId;
        rigaOrdine.Quantita = quantita;
        rigaOrdine.PrezzoUnitario = prezzoUnitario;
        rigaOrdine.SetConcurrencyStampIfNotNull(concurrencyStamp);
        return await _rigaOrdineRepository.UpdateAsync(rigaOrdine);
    }
}