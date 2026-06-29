using LFG.Categorie;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using JetBrains.Annotations;
using LFG.VarianteProdotti;
using Volo.Abp;

namespace LFG.Prodotti;

public abstract class ProdottoBase : FullAuditedAggregateRoot<Guid>
{
    [NotNull]
    public virtual string Nome { get; set; }

    [CanBeNull]
    public virtual string? Descrizione { get; set; }

    [NotNull]
    public virtual string Prezzo { get; set; }

    [CanBeNull]
    public virtual string? CodiceSku { get; set; }

    [NotNull]
    public virtual string Sezione { get; set; }

    public Guid? CategoriaId { get; set; }

    public ICollection<ProdottoColleziones> Colleziones { get; protected set; }

    public ICollection<VarianteProdotto> VarianteProdotti { get; protected set; } = new Collection<VarianteProdotto>();

    protected ProdottoBase()
    {
    }

    public ProdottoBase(Guid id, Guid? categoriaId, string nome, string prezzo, string sezione, string? descrizione = null, string? codiceSku = null)
    {
        Id = id;
        Check.NotNull(nome, nameof(nome));
        Check.Length(nome, nameof(nome), ProdottoConsts.NomeMaxLength, ProdottoConsts.NomeMinLength);
        Check.NotNull(prezzo, nameof(prezzo));
        Check.NotNull(sezione, nameof(sezione));
        Check.Length(sezione, nameof(sezione), ProdottoConsts.SezioneMaxLength, ProdottoConsts.SezioneMinLength);
        Check.Length(descrizione, nameof(descrizione), ProdottoConsts.DescrizioneMaxLength, 0);
        Check.Length(codiceSku, nameof(codiceSku), ProdottoConsts.CodiceSkuMaxLength, 0);
        Nome = nome;
        Prezzo = prezzo;
        Sezione = sezione;
        Descrizione = descrizione;
        CodiceSku = codiceSku;
        CategoriaId = categoriaId;
        Colleziones = new Collection<ProdottoColleziones>();
    }

    public virtual void AddToColleziones(Guid collezioneId)
    {
        Check.NotNull(collezioneId, nameof(collezioneId));
        if (IsInColleziones(collezioneId))
        {
            return;
        }

        Colleziones.Add(new ProdottoColleziones(Id, collezioneId));
    }

    public virtual void RemoveFromColleziones(Guid collezioneId)
    {
        Check.NotNull(collezioneId, nameof(collezioneId));
        if (!IsInColleziones(collezioneId))
        {
            return;
        }

        Colleziones.RemoveAll(x => x.CollezioneId == collezioneId);
    }

    public virtual void RemoveAllCollezionesExceptGivenIds(List<Guid> collezioneIds)
    {
        Check.NotNullOrEmpty(collezioneIds, nameof(collezioneIds));
        Colleziones.RemoveAll(x => !collezioneIds.Contains(x.CollezioneId));
    }

    public virtual void RemoveAllColleziones()
    {
        Colleziones.RemoveAll(x => x.ProdottoId == Id);
    }

    private bool IsInColleziones(Guid collezioneId)
    {
        return Colleziones.Any(x => x.CollezioneId == collezioneId);
    }
}