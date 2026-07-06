using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using JetBrains.Annotations;
using Volo.Abp;

namespace LFG.Sconti;

public abstract class ScontoBase : FullAuditedAggregateRoot<Guid>
{
    [NotNull]
    public virtual string Codice { get; set; }

    [CanBeNull]
    public virtual string? Tipo { get; set; }

    public virtual decimal Valore { get; set; }

    public virtual int? LimiteUtilizzi { get; set; }

    public virtual DateTime ValidoDal { get; set; }

    public virtual DateTime ValidoAl { get; set; }

    [NotNull]
    public virtual string Sezione { get; set; }

    protected ScontoBase()
    {
    }

    public ScontoBase(Guid id, string codice, decimal valore, DateTime validoDal, DateTime validoAl, string sezione, string? tipo = null, int? limiteUtilizzi = null)
    {
        Id = id;
        Check.NotNull(codice, nameof(codice));
        Check.Length(codice, nameof(codice), ScontoConsts.CodiceMaxLength, ScontoConsts.CodiceMinLength);
        Check.NotNull(sezione, nameof(sezione));
        Check.Length(sezione, nameof(sezione), ScontoConsts.SezioneMaxLength, ScontoConsts.SezioneMinLength);
        Codice = codice;
        Valore = valore;
        ValidoDal = validoDal;
        ValidoAl = validoAl;
        Sezione = sezione;
        Tipo = tipo;
        LimiteUtilizzi = limiteUtilizzi;
    }
}