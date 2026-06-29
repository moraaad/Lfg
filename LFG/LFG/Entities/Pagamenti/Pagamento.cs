using LFG.Ordini;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using JetBrains.Annotations;
using Volo.Abp;

namespace LFG.Pagamenti;

public abstract class PagamentoBase : FullAuditedAggregateRoot<Guid>
{
    [CanBeNull]
    public virtual string? Metodo { get; set; }

    [CanBeNull]
    public virtual string? Stato { get; set; }

    public virtual decimal Importo { get; set; }

    public virtual DateTime DataPagamento { get; set; }

    [NotNull]
    public virtual string IdTransazione { get; set; }

    public Guid? OrdineId { get; set; }

    protected PagamentoBase()
    {
    }

    public PagamentoBase(Guid id, Guid? ordineId, decimal importo, DateTime dataPagamento, string idTransazione, string? metodo = null, string? stato = null)
    {
        Id = id;
        Check.NotNull(idTransazione, nameof(idTransazione));
        Check.Length(idTransazione, nameof(idTransazione), PagamentoConsts.IdTransazioneMaxLength, 0);
        Importo = importo;
        DataPagamento = dataPagamento;
        IdTransazione = idTransazione;
        Metodo = metodo;
        Stato = stato;
        OrdineId = ordineId;
    }
}