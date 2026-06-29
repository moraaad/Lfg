using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using JetBrains.Annotations;
using Volo.Abp;

namespace LFG.Collezioni;

public abstract class CollezioneBase : FullAuditedAggregateRoot<Guid>
{
    [NotNull]
    public virtual string Nome { get; set; }

    [NotNull]
    public virtual string Stagione { get; set; }

    public virtual DateTime Anno { get; set; }

    [NotNull]
    public virtual string Sezione { get; set; }

    protected CollezioneBase()
    {
    }

    public CollezioneBase(Guid id, string nome, string stagione, DateTime anno, string sezione)
    {
        Id = id;
        Check.NotNull(nome, nameof(nome));
        Check.Length(nome, nameof(nome), CollezioneConsts.NomeMaxLength, CollezioneConsts.NomeMinLength);
        Check.NotNull(stagione, nameof(stagione));
        Check.Length(stagione, nameof(stagione), CollezioneConsts.StagioneMaxLength, CollezioneConsts.StagioneMinLength);
        Check.NotNull(sezione, nameof(sezione));
        Check.Length(sezione, nameof(sezione), CollezioneConsts.SezioneMaxLength, CollezioneConsts.SezioneMinLength);
        Nome = nome;
        Stagione = stagione;
        Anno = anno;
        Sezione = sezione;
    }
}