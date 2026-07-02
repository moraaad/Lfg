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

namespace LFG.ImmagineVarianti;

public abstract class ImmagineVarianteBase : FullAuditedAggregateRoot<Guid>
{
    public virtual Guid VarianteProdottoId { get; set; }

    [NotNull]
    public virtual string Url { get; set; }

    public virtual int Ordine { get; set; }

    protected ImmagineVarianteBase()
    {
    }

    public ImmagineVarianteBase(Guid id, Guid varianteProdottoId, string url, int ordine)
    {
        Id = id;
        Check.NotNull(url, nameof(url));
        Check.Length(url, nameof(url), ImmagineVarianteConsts.UrlMaxLength, 0);
        VarianteProdottoId = varianteProdottoId;
        Url = url;
        Ordine = ordine;
    }
}