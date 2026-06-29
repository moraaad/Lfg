using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using JetBrains.Annotations;
using Volo.Abp;

namespace LFG.Clienti;

public abstract class ClienteBase : FullAuditedAggregateRoot<Guid>
{
    [NotNull]
    public virtual string Nome { get; set; }

    [NotNull]
    public virtual string Cognome { get; set; }

    [NotNull]
    public virtual string Genere { get; set; }

    public virtual DateTime? DataNascita { get; set; }

    [NotNull]
    public virtual string Email { get; set; }

    [NotNull]
    public virtual string Telefono { get; set; }

    [NotNull]
    public virtual string Sezione { get; set; }

    [NotNull]
    public virtual string Nazionalita { get; set; }

    protected ClienteBase()
    {
    }

    public ClienteBase(Guid id, string nome, string cognome, string genere, string email, string telefono, string sezione, string nazionalita, DateTime? dataNascita = null)
    {
        Id = id;
        Check.NotNull(nome, nameof(nome));
        Check.Length(nome, nameof(nome), ClienteConsts.NomeMaxLength, ClienteConsts.NomeMinLength);
        Check.NotNull(cognome, nameof(cognome));
        Check.Length(cognome, nameof(cognome), ClienteConsts.CognomeMaxLength, ClienteConsts.CognomeMinLength);
        Check.NotNull(genere, nameof(genere));
        Check.NotNull(email, nameof(email));
        Check.Length(email, nameof(email), ClienteConsts.EmailMaxLength, ClienteConsts.EmailMinLength);
        Check.NotNull(telefono, nameof(telefono));
        Check.Length(telefono, nameof(telefono), ClienteConsts.TelefonoMaxLength, ClienteConsts.TelefonoMinLength);
        Check.NotNull(sezione, nameof(sezione));
        Check.Length(sezione, nameof(sezione), ClienteConsts.SezioneMaxLength, ClienteConsts.SezioneMinLength);
        Check.NotNull(nazionalita, nameof(nazionalita));
        Nome = nome;
        Cognome = cognome;
        Genere = genere;
        Email = email;
        Telefono = telefono;
        Sezione = sezione;
        Nazionalita = nazionalita;
        DataNascita = dataNascita;
    }
}