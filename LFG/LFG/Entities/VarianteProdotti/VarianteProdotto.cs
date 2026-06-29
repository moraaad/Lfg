using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using JetBrains.Annotations;
using Volo.Abp;

namespace LFG.VarianteProdotti;

public abstract class VarianteProdottoBase : FullAuditedEntity<Guid>
{
    public virtual Guid ProdottoId { get; set; }

    [CanBeNull]
    public virtual string? Taglia { get; set; }

    [CanBeNull]
    public virtual string? Colore { get; set; }

    [CanBeNull]
    public virtual string? Materiale { get; set; }

    [CanBeNull]
    public virtual string? UrlImmagine { get; set; }

    public virtual int QtaMagazzino { get; set; }

    protected VarianteProdottoBase()
    {
    }

    public VarianteProdottoBase(Guid id, Guid prodottoId, int qtaMagazzino, string? taglia = null, string? colore = null, string? materiale = null, string? urlImmagine = null)
    {
        Id = id;
        Check.Length(urlImmagine, nameof(urlImmagine), VarianteProdottoConsts.UrlImmagineMaxLength, 0);
        ProdottoId = prodottoId;
        QtaMagazzino = qtaMagazzino;
        Taglia = taglia;
        Colore = colore;
        Materiale = materiale;
        UrlImmagine = urlImmagine;
    }
}