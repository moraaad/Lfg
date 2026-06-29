using LFG.VarianteProdotti;
using LFG.ListeDesideri;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using JetBrains.Annotations;
using Volo.Abp;

namespace LFG.ElementoListe;

public abstract class ElementoListaBase : FullAuditedAggregateRoot<Guid>
{
    public virtual DateTime DataAggiunta { get; set; }

    public Guid VarianteProdottoId { get; set; }

    public Guid? ListaDesideriId { get; set; }

    protected ElementoListaBase()
    {
    }

    public ElementoListaBase(Guid id, Guid varianteProdottoId, Guid? listaDesideriId, DateTime dataAggiunta)
    {
        Id = id;
        DataAggiunta = dataAggiunta;
        VarianteProdottoId = varianteProdottoId;
        ListaDesideriId = listaDesideriId;
    }
}