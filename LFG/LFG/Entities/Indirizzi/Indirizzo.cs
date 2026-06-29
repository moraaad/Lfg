using LFG.Clienti;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using JetBrains.Annotations;
using Volo.Abp;

namespace LFG.Indirizzi;

public abstract class IndirizzoBase : FullAuditedAggregateRoot<Guid>
{
    [CanBeNull]
    public virtual string? Paese { get; set; }

    [CanBeNull]
    public virtual string? Citta { get; set; }

    [CanBeNull]
    public virtual string? Provincia { get; set; }

    [NotNull]
    public virtual string Via { get; set; }

    [NotNull]
    public virtual string Cap { get; set; }

    public Guid? ClienteId { get; set; }

    protected IndirizzoBase()
    {
    }

    public IndirizzoBase(Guid id, Guid? clienteId, string via, string cap, string? paese = null, string? citta = null, string? provincia = null)
    {
        Id = id;
        Check.NotNull(via, nameof(via));
        Check.Length(via, nameof(via), IndirizzoConsts.ViaMaxLength, 0);
        Check.NotNull(cap, nameof(cap));
        Check.Length(cap, nameof(cap), IndirizzoConsts.CapMaxLength, IndirizzoConsts.CapMinLength);
        Via = via;
        Cap = cap;
        Paese = paese;
        Citta = citta;
        Provincia = provincia;
        ClienteId = clienteId;
    }
}