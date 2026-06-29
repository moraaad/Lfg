using LFG.Ordini;
using LFG.VarianteProdotti;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using JetBrains.Annotations;
using Volo.Abp;

namespace LFG.RigaOrdini;

public abstract class RigaOrdineBase : FullAuditedAggregateRoot<Guid>
{
    public virtual int Quantita { get; set; }

    public virtual decimal PrezzoUnitario { get; set; }

    public Guid? OrdineId { get; set; }

    public Guid? VarianteProdottoId { get; set; }

    protected RigaOrdineBase()
    {
    }

    public RigaOrdineBase(Guid id, Guid? ordineId, Guid? varianteProdottoId, int quantita, decimal prezzoUnitario)
    {
        Id = id;
        if (quantita < RigaOrdineConsts.QuantitaMinLength)
        {
            throw new ArgumentOutOfRangeException(nameof(quantita), quantita, "The value of 'quantita' cannot be lower than " + RigaOrdineConsts.QuantitaMinLength);
        }

        if (quantita > RigaOrdineConsts.QuantitaMaxLength)
        {
            throw new ArgumentOutOfRangeException(nameof(quantita), quantita, "The value of 'quantita' cannot be greater than " + RigaOrdineConsts.QuantitaMaxLength);
        }

        if (prezzoUnitario < RigaOrdineConsts.PrezzoUnitarioMinLength)
        {
            throw new ArgumentOutOfRangeException(nameof(prezzoUnitario), prezzoUnitario, "The value of 'prezzoUnitario' cannot be lower than " + RigaOrdineConsts.PrezzoUnitarioMinLength);
        }

        if (prezzoUnitario > RigaOrdineConsts.PrezzoUnitarioMaxLength)
        {
            throw new ArgumentOutOfRangeException(nameof(prezzoUnitario), prezzoUnitario, "The value of 'prezzoUnitario' cannot be greater than " + RigaOrdineConsts.PrezzoUnitarioMaxLength);
        }

        Quantita = quantita;
        PrezzoUnitario = prezzoUnitario;
        OrdineId = ordineId;
        VarianteProdottoId = varianteProdottoId;
    }
}