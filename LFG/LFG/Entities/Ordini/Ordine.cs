using LFG.Clienti;
using LFG.Sconti;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using JetBrains.Annotations;
using Volo.Abp;

namespace LFG.Ordini;

public abstract class OrdineBase : FullAuditedAggregateRoot<Guid>
{
    public virtual DateTime DataOrdine { get; set; }

    [CanBeNull]
    public virtual string? Stato { get; set; }

    public virtual decimal ImportoTotale { get; set; }

    [CanBeNull]
    public virtual string? IndSpedizione { get; set; }

    [CanBeNull]
    public virtual string? MetodoPagamento { get; set; }

    public Guid? ClienteId { get; set; }

    public Guid? ScontoId { get; set; }

    protected OrdineBase()
    {
    }

    public OrdineBase(Guid id, Guid? clienteId, Guid? scontoId, DateTime dataOrdine, decimal importoTotale, string? stato = null, string? indSpedizione = null, string? metodoPagamento = null)
    {
        Id = id;
        Check.Length(stato, nameof(stato), OrdineConsts.StatoMaxLength, 0);
        Check.Length(indSpedizione, nameof(indSpedizione), OrdineConsts.IndSpedizioneMaxLength, 0);
        Check.Length(metodoPagamento, nameof(metodoPagamento), OrdineConsts.MetodoPagamentoMaxLength, 0);
        DataOrdine = dataOrdine;
        ImportoTotale = importoTotale;
        Stato = stato;
        IndSpedizione = indSpedizione;
        MetodoPagamento = metodoPagamento;
        ClienteId = clienteId;
        ScontoId = scontoId;
    }
}