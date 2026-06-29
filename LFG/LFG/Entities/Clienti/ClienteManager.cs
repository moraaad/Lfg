using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Data;

namespace LFG.Clienti;

public abstract class ClienteManagerBase : DomainService
{
    protected IClienteRepository _clienteRepository;

    public ClienteManagerBase(IClienteRepository clienteRepository)
    {
        _clienteRepository = clienteRepository;
    }

    public virtual async Task<Cliente> CreateAsync(string nome, string cognome, string genere, string email, string telefono, string sezione, string nazionalita, DateTime? dataNascita = null)
    {
        Check.NotNullOrWhiteSpace(nome, nameof(nome));
        Check.Length(nome, nameof(nome), ClienteConsts.NomeMaxLength, ClienteConsts.NomeMinLength);
        Check.NotNullOrWhiteSpace(cognome, nameof(cognome));
        Check.Length(cognome, nameof(cognome), ClienteConsts.CognomeMaxLength, ClienteConsts.CognomeMinLength);
        Check.NotNullOrWhiteSpace(genere, nameof(genere));
        Check.NotNullOrWhiteSpace(email, nameof(email));
        Check.Length(email, nameof(email), ClienteConsts.EmailMaxLength, ClienteConsts.EmailMinLength);
        Check.NotNullOrWhiteSpace(telefono, nameof(telefono));
        Check.Length(telefono, nameof(telefono), ClienteConsts.TelefonoMaxLength, ClienteConsts.TelefonoMinLength);
        Check.NotNullOrWhiteSpace(sezione, nameof(sezione));
        Check.Length(sezione, nameof(sezione), ClienteConsts.SezioneMaxLength, ClienteConsts.SezioneMinLength);
        Check.NotNullOrWhiteSpace(nazionalita, nameof(nazionalita));
        var cliente = new Cliente(GuidGenerator.Create(), nome, cognome, genere, email, telefono, sezione, nazionalita, dataNascita);
        return await _clienteRepository.InsertAsync(cliente);
    }

    public virtual async Task<Cliente> UpdateAsync(Guid id, string nome, string cognome, string genere, string email, string telefono, string sezione, string nazionalita, DateTime? dataNascita = null, [CanBeNull] string? concurrencyStamp = null)
    {
        Check.NotNullOrWhiteSpace(nome, nameof(nome));
        Check.Length(nome, nameof(nome), ClienteConsts.NomeMaxLength, ClienteConsts.NomeMinLength);
        Check.NotNullOrWhiteSpace(cognome, nameof(cognome));
        Check.Length(cognome, nameof(cognome), ClienteConsts.CognomeMaxLength, ClienteConsts.CognomeMinLength);
        Check.NotNullOrWhiteSpace(genere, nameof(genere));
        Check.NotNullOrWhiteSpace(email, nameof(email));
        Check.Length(email, nameof(email), ClienteConsts.EmailMaxLength, ClienteConsts.EmailMinLength);
        Check.NotNullOrWhiteSpace(telefono, nameof(telefono));
        Check.Length(telefono, nameof(telefono), ClienteConsts.TelefonoMaxLength, ClienteConsts.TelefonoMinLength);
        Check.NotNullOrWhiteSpace(sezione, nameof(sezione));
        Check.Length(sezione, nameof(sezione), ClienteConsts.SezioneMaxLength, ClienteConsts.SezioneMinLength);
        Check.NotNullOrWhiteSpace(nazionalita, nameof(nazionalita));
        var cliente = await _clienteRepository.GetAsync(id);
        cliente.Nome = nome;
        cliente.Cognome = cognome;
        cliente.Genere = genere;
        cliente.Email = email;
        cliente.Telefono = telefono;
        cliente.Sezione = sezione;
        cliente.Nazionalita = nazionalita;
        cliente.DataNascita = dataNascita;
        cliente.SetConcurrencyStampIfNotNull(concurrencyStamp);
        return await _clienteRepository.UpdateAsync(cliente);
    }
}