using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Data;

namespace LFG.Recensioni;

public abstract class RecensioneManagerBase : DomainService
{
    protected IRecensioneRepository _recensioneRepository;

    public RecensioneManagerBase(IRecensioneRepository recensioneRepository)
    {
        _recensioneRepository = recensioneRepository;
    }

    public virtual async Task<Recensione> CreateAsync(Guid? clienteId, Guid? prodottoId, int valutazione, DateTime dataRecensione, string? commento = null)
    {
        Check.Range(valutazione, nameof(valutazione), RecensioneConsts.ValutazioneMinLength, RecensioneConsts.ValutazioneMaxLength);
        Check.NotNull(dataRecensione, nameof(dataRecensione));
        Check.Length(commento, nameof(commento), RecensioneConsts.CommentoMaxLength);
        var recensione = new Recensione(GuidGenerator.Create(), clienteId, prodottoId, valutazione, dataRecensione, commento);
        return await _recensioneRepository.InsertAsync(recensione);
    }

    public virtual async Task<Recensione> UpdateAsync(Guid id, Guid? clienteId, Guid? prodottoId, int valutazione, DateTime dataRecensione, string? commento = null, [CanBeNull] string? concurrencyStamp = null)
    {
        Check.Range(valutazione, nameof(valutazione), RecensioneConsts.ValutazioneMinLength, RecensioneConsts.ValutazioneMaxLength);
        Check.NotNull(dataRecensione, nameof(dataRecensione));
        Check.Length(commento, nameof(commento), RecensioneConsts.CommentoMaxLength);
        var recensione = await _recensioneRepository.GetAsync(id);
        recensione.ClienteId = clienteId;
        recensione.ProdottoId = prodottoId;
        recensione.Valutazione = valutazione;
        recensione.DataRecensione = dataRecensione;
        recensione.Commento = commento;
        recensione.SetConcurrencyStampIfNotNull(concurrencyStamp);
        return await _recensioneRepository.UpdateAsync(recensione);
    }
}