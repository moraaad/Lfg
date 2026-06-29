using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Data;

namespace LFG.Pagamenti;

public abstract class PagamentoManagerBase : DomainService
{
    protected IPagamentoRepository _pagamentoRepository;

    public PagamentoManagerBase(IPagamentoRepository pagamentoRepository)
    {
        _pagamentoRepository = pagamentoRepository;
    }

    public virtual async Task<Pagamento> CreateAsync(Guid? ordineId, decimal importo, DateTime dataPagamento, string idTransazione, string? metodo = null, string? stato = null)
    {
        Check.NotNull(dataPagamento, nameof(dataPagamento));
        Check.NotNullOrWhiteSpace(idTransazione, nameof(idTransazione));
        Check.Length(idTransazione, nameof(idTransazione), PagamentoConsts.IdTransazioneMaxLength);
        var pagamento = new Pagamento(GuidGenerator.Create(), ordineId, importo, dataPagamento, idTransazione, metodo, stato);
        return await _pagamentoRepository.InsertAsync(pagamento);
    }

    public virtual async Task<Pagamento> UpdateAsync(Guid id, Guid? ordineId, decimal importo, DateTime dataPagamento, string idTransazione, string? metodo = null, string? stato = null, [CanBeNull] string? concurrencyStamp = null)
    {
        Check.NotNull(dataPagamento, nameof(dataPagamento));
        Check.NotNullOrWhiteSpace(idTransazione, nameof(idTransazione));
        Check.Length(idTransazione, nameof(idTransazione), PagamentoConsts.IdTransazioneMaxLength);
        var pagamento = await _pagamentoRepository.GetAsync(id);
        pagamento.OrdineId = ordineId;
        pagamento.Importo = importo;
        pagamento.DataPagamento = dataPagamento;
        pagamento.IdTransazione = idTransazione;
        pagamento.Metodo = metodo;
        pagamento.Stato = stato;
        pagamento.SetConcurrencyStampIfNotNull(concurrencyStamp);
        return await _pagamentoRepository.UpdateAsync(pagamento);
    }
}