using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Data;

namespace LFG.Ordini;

public abstract class OrdineManagerBase : DomainService
{
    protected IOrdineRepository _ordineRepository;

    public OrdineManagerBase(IOrdineRepository ordineRepository)
    {
        _ordineRepository = ordineRepository;
    }

    public virtual async Task<Ordine> CreateAsync(Guid? clienteId, Guid? scontoId, DateTime dataOrdine, decimal importoTotale, string? stato = null, string? indSpedizione = null, string? metodoPagamento = null)
    {
        Check.NotNull(dataOrdine, nameof(dataOrdine));
        Check.Length(stato, nameof(stato), OrdineConsts.StatoMaxLength);
        Check.Length(indSpedizione, nameof(indSpedizione), OrdineConsts.IndSpedizioneMaxLength);
        Check.Length(metodoPagamento, nameof(metodoPagamento), OrdineConsts.MetodoPagamentoMaxLength);
        var ordine = new Ordine(GuidGenerator.Create(), clienteId, scontoId, dataOrdine, importoTotale, stato, indSpedizione, metodoPagamento);
        return await _ordineRepository.InsertAsync(ordine);
    }

    public virtual async Task<Ordine> UpdateAsync(Guid id, Guid? clienteId, Guid? scontoId, DateTime dataOrdine, decimal importoTotale, string? stato = null, string? indSpedizione = null, string? metodoPagamento = null, [CanBeNull] string? concurrencyStamp = null)
    {
        Check.NotNull(dataOrdine, nameof(dataOrdine));
        Check.Length(stato, nameof(stato), OrdineConsts.StatoMaxLength);
        Check.Length(indSpedizione, nameof(indSpedizione), OrdineConsts.IndSpedizioneMaxLength);
        Check.Length(metodoPagamento, nameof(metodoPagamento), OrdineConsts.MetodoPagamentoMaxLength);
        var ordine = await _ordineRepository.GetAsync(id);
        ordine.ClienteId = clienteId;
        ordine.ScontoId = scontoId;
        ordine.DataOrdine = dataOrdine;
        ordine.ImportoTotale = importoTotale;
        ordine.Stato = stato;
        ordine.IndSpedizione = indSpedizione;
        ordine.MetodoPagamento = metodoPagamento;
        ordine.SetConcurrencyStampIfNotNull(concurrencyStamp);
        return await _ordineRepository.UpdateAsync(ordine);
    }
}