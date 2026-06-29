using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using JetBrains.Annotations;
using Volo.Abp;

namespace LFG.Categorie;

public abstract class CategoriaBase : FullAuditedAggregateRoot<Guid>
{
    [NotNull]
    public virtual string Nome { get; set; }

    [CanBeNull]
    public virtual string? Descrizione { get; set; }

    [NotNull]
    public virtual string Sezione { get; set; }

    protected CategoriaBase()
    {
    }

    public CategoriaBase(Guid id, string nome, string sezione, string? descrizione = null)
    {
        Id = id;
        Check.NotNull(nome, nameof(nome));
        Check.Length(nome, nameof(nome), CategoriaConsts.NomeMaxLength, CategoriaConsts.NomeMinLength);
        Check.NotNull(sezione, nameof(sezione));
        Check.Length(sezione, nameof(sezione), CategoriaConsts.SezioneMaxLength, CategoriaConsts.SezioneMinLength);
        Check.Length(descrizione, nameof(descrizione), CategoriaConsts.DescrizioneMaxLength, 0);
        Nome = nome;
        Sezione = sezione;
        Descrizione = descrizione;
    }
}