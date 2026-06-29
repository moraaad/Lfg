using System;
using Volo.Abp.Domain.Entities;

namespace LFG.Prodotti;

public class ProdottoColleziones : Entity
{
    public Guid ProdottoId { get; protected set; }

    public Guid CollezioneId { get; protected set; }

    private ProdottoColleziones()
    {
    }

    public ProdottoColleziones(Guid prodottoId, Guid collezioneId)
    {
        ProdottoId = prodottoId;
        CollezioneId = collezioneId;
    }

    public override object[] GetKeys()
    {
        return new object[] {
            ProdottoId,
            CollezioneId
        };
    }
}