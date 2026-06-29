using LFG.Clienti;
using LFG.Prodotti;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using JetBrains.Annotations;
using Volo.Abp;

namespace LFG.Recensioni;

public abstract class RecensioneBase : FullAuditedAggregateRoot<Guid>
{
    public virtual int Valutazione { get; set; }

    [CanBeNull]
    public virtual string? Commento { get; set; }

    public virtual DateTime DataRecensione { get; set; }

    public Guid? ClienteId { get; set; }

    public Guid? ProdottoId { get; set; }

    protected RecensioneBase()
    {
    }

    public RecensioneBase(Guid id, Guid? clienteId, Guid? prodottoId, int valutazione, DateTime dataRecensione, string? commento = null)
    {
        Id = id;
        if (valutazione < RecensioneConsts.ValutazioneMinLength)
        {
            throw new ArgumentOutOfRangeException(nameof(valutazione), valutazione, "The value of 'valutazione' cannot be lower than " + RecensioneConsts.ValutazioneMinLength);
        }

        if (valutazione > RecensioneConsts.ValutazioneMaxLength)
        {
            throw new ArgumentOutOfRangeException(nameof(valutazione), valutazione, "The value of 'valutazione' cannot be greater than " + RecensioneConsts.ValutazioneMaxLength);
        }

        Check.Length(commento, nameof(commento), RecensioneConsts.CommentoMaxLength, 0);
        Valutazione = valutazione;
        DataRecensione = dataRecensione;
        Commento = commento;
        ClienteId = clienteId;
        ProdottoId = prodottoId;
    }
}