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

namespace LFG.ListeDesideri;

public abstract class ListaDesideriBase : FullAuditedAggregateRoot<Guid>
{
    public virtual DateTime DataCreazione { get; set; }

    [NotNull]
    public virtual string NomeLista { get; set; }

    public Guid? ClienteId { get; set; }

    protected ListaDesideriBase()
    {
    }

    public ListaDesideriBase(Guid id, Guid? clienteId, DateTime dataCreazione, string nomeLista)
    {
        Id = id;
        Check.NotNull(nomeLista, nameof(nomeLista));
        Check.Length(nomeLista, nameof(nomeLista), ListaDesideriConsts.NomeListaMaxLength, ListaDesideriConsts.NomeListaMinLength);
        DataCreazione = dataCreazione;
        NomeLista = nomeLista;
        ClienteId = clienteId;
    }
}