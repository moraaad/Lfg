using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Data;

namespace LFG.Indirizzi;

public abstract class IndirizzoManagerBase : DomainService
{
    protected IIndirizzoRepository _indirizzoRepository;

    public IndirizzoManagerBase(IIndirizzoRepository indirizzoRepository)
    {
        _indirizzoRepository = indirizzoRepository;
    }

    public virtual async Task<Indirizzo> CreateAsync(Guid? clienteId, string via, string cap, string? paese = null, string? citta = null, string? provincia = null)
    {
        Check.NotNullOrWhiteSpace(via, nameof(via));
        Check.Length(via, nameof(via), IndirizzoConsts.ViaMaxLength);
        Check.NotNullOrWhiteSpace(cap, nameof(cap));
        Check.Length(cap, nameof(cap), IndirizzoConsts.CapMaxLength, IndirizzoConsts.CapMinLength);
        var indirizzo = new Indirizzo(GuidGenerator.Create(), clienteId, via, cap, paese, citta, provincia);
        return await _indirizzoRepository.InsertAsync(indirizzo);
    }

    public virtual async Task<Indirizzo> UpdateAsync(Guid id, Guid? clienteId, string via, string cap, string? paese = null, string? citta = null, string? provincia = null, [CanBeNull] string? concurrencyStamp = null)
    {
        Check.NotNullOrWhiteSpace(via, nameof(via));
        Check.Length(via, nameof(via), IndirizzoConsts.ViaMaxLength);
        Check.NotNullOrWhiteSpace(cap, nameof(cap));
        Check.Length(cap, nameof(cap), IndirizzoConsts.CapMaxLength, IndirizzoConsts.CapMinLength);
        var indirizzo = await _indirizzoRepository.GetAsync(id);
        indirizzo.ClienteId = clienteId;
        indirizzo.Via = via;
        indirizzo.Cap = cap;
        indirizzo.Paese = paese;
        indirizzo.Citta = citta;
        indirizzo.Provincia = provincia;
        indirizzo.SetConcurrencyStampIfNotNull(concurrencyStamp);
        return await _indirizzoRepository.UpdateAsync(indirizzo);
    }
}