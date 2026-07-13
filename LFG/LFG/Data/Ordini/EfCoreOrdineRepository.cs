using LFG.Indirizzi;
using LFG.Sconti;
using LFG.Clienti;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using LFG.Data;

namespace LFG.Ordini;

public abstract class EfCoreOrdineRepositoryBase : EfCoreRepository<LFGDbContext, Ordine, Guid>
{
    public EfCoreOrdineRepositoryBase(IDbContextProvider<LFGDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public virtual async Task DeleteAllAsync(string? filterText = null, DateTime? dataOrdineMin = null, DateTime? dataOrdineMax = null, string? stato = null, decimal? importoTotaleMin = null, decimal? importoTotaleMax = null, string? indSpedizione = null, string? metodoPagamento = null, Guid? clienteId = null, Guid? scontoId = null, Guid? indirizzoId = null, CancellationToken cancellationToken = default)
    {
        var query = await GetQueryForNavigationPropertiesAsync();
        query = ApplyFilter(query, filterText, dataOrdineMin, dataOrdineMax, stato, importoTotaleMin, importoTotaleMax, indSpedizione, metodoPagamento, clienteId, scontoId, indirizzoId);
        var ids = query.Select(x => x.Ordine.Id);
        await DeleteManyAsync(ids, cancellationToken: GetCancellationToken(cancellationToken));
    }

    public virtual async Task<OrdineWithNavigationProperties> GetWithNavigationPropertiesAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        return (await GetDbSetAsync()).Where(b => b.Id == id).Select(ordine => new OrdineWithNavigationProperties { Ordine = ordine, Cliente = dbContext.Set<Cliente>().FirstOrDefault(c => c.Id == ordine.ClienteId), Sconto = dbContext.Set<Sconto>().FirstOrDefault(c => c.Id == ordine.ScontoId), Indirizzo = dbContext.Set<Indirizzo>().FirstOrDefault(c => c.Id == ordine.IndirizzoId) }).FirstOrDefault();
    }

    public virtual async Task<List<OrdineWithNavigationProperties>> GetListWithNavigationPropertiesAsync(string? filterText = null, DateTime? dataOrdineMin = null, DateTime? dataOrdineMax = null, string? stato = null, decimal? importoTotaleMin = null, decimal? importoTotaleMax = null, string? indSpedizione = null, string? metodoPagamento = null, Guid? clienteId = null, Guid? scontoId = null, Guid? indirizzoId = null, string? sorting = null, int maxResultCount = int.MaxValue, int skipCount = 0, CancellationToken cancellationToken = default)
    {
        var query = await GetQueryForNavigationPropertiesAsync();
        query = ApplyFilter(query, filterText, dataOrdineMin, dataOrdineMax, stato, importoTotaleMin, importoTotaleMax, indSpedizione, metodoPagamento, clienteId, scontoId, indirizzoId);
        query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? OrdineConsts.GetDefaultSorting(true) : sorting);
        return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
    }

    protected virtual async Task<IQueryable<OrdineWithNavigationProperties>> GetQueryForNavigationPropertiesAsync()
    {
        return from ordine in (await GetDbSetAsync())
               join cliente in (await GetDbContextAsync()).Set<Cliente>() on ordine.ClienteId equals cliente.Id into clientes
               from cliente in clientes.DefaultIfEmpty()
               join sconto in (await GetDbContextAsync()).Set<Sconto>() on ordine.ScontoId equals sconto.Id into scontos
               from sconto in scontos.DefaultIfEmpty()
               join indirizzo in (await GetDbContextAsync()).Set<Indirizzo>() on ordine.IndirizzoId equals indirizzo.Id into indirizzos
               from indirizzo in indirizzos.DefaultIfEmpty()
               select new OrdineWithNavigationProperties
               {
                   Ordine = ordine,
                   Cliente = cliente,
                   Sconto = sconto,
                   Indirizzo = indirizzo
               };
    }

    protected virtual IQueryable<OrdineWithNavigationProperties> ApplyFilter(IQueryable<OrdineWithNavigationProperties> query, string? filterText, DateTime? dataOrdineMin = null, DateTime? dataOrdineMax = null, string? stato = null, decimal? importoTotaleMin = null, decimal? importoTotaleMax = null, string? indSpedizione = null, string? metodoPagamento = null, Guid? clienteId = null, Guid? scontoId = null, Guid? indirizzoId = null)
    {
        return query.WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.Ordine.Stato!.Contains(filterText!) || e.Ordine.IndSpedizione!.Contains(filterText!) || e.Ordine.MetodoPagamento!.Contains(filterText!)).WhereIf(dataOrdineMin.HasValue, e => e.Ordine.DataOrdine >= dataOrdineMin!.Value).WhereIf(dataOrdineMax.HasValue, e => e.Ordine.DataOrdine <= dataOrdineMax!.Value).WhereIf(!string.IsNullOrWhiteSpace(stato), e => e.Ordine.Stato.Contains(stato)).WhereIf(importoTotaleMin.HasValue, e => e.Ordine.ImportoTotale >= importoTotaleMin!.Value).WhereIf(importoTotaleMax.HasValue, e => e.Ordine.ImportoTotale <= importoTotaleMax!.Value).WhereIf(!string.IsNullOrWhiteSpace(indSpedizione), e => e.Ordine.IndSpedizione.Contains(indSpedizione)).WhereIf(!string.IsNullOrWhiteSpace(metodoPagamento), e => e.Ordine.MetodoPagamento.Contains(metodoPagamento)).WhereIf(clienteId != null && clienteId != Guid.Empty, e => e.Cliente != null && e.Cliente.Id == clienteId).WhereIf(scontoId != null && scontoId != Guid.Empty, e => e.Sconto != null && e.Sconto.Id == scontoId).WhereIf(indirizzoId != null && indirizzoId != Guid.Empty, e => e.Indirizzo != null && e.Indirizzo.Id == indirizzoId);
    }

    public virtual async Task<List<Ordine>> GetListAsync(string? filterText = null, DateTime? dataOrdineMin = null, DateTime? dataOrdineMax = null, string? stato = null, decimal? importoTotaleMin = null, decimal? importoTotaleMax = null, string? indSpedizione = null, string? metodoPagamento = null, string? sorting = null, int maxResultCount = int.MaxValue, int skipCount = 0, CancellationToken cancellationToken = default)
    {
        var query = ApplyFilter((await GetQueryableAsync()), filterText, dataOrdineMin, dataOrdineMax, stato, importoTotaleMin, importoTotaleMax, indSpedizione, metodoPagamento);
        query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? OrdineConsts.GetDefaultSorting(false) : sorting);
        return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
    }

    public virtual async Task<long> GetCountAsync(string? filterText = null, DateTime? dataOrdineMin = null, DateTime? dataOrdineMax = null, string? stato = null, decimal? importoTotaleMin = null, decimal? importoTotaleMax = null, string? indSpedizione = null, string? metodoPagamento = null, Guid? clienteId = null, Guid? scontoId = null, Guid? indirizzoId = null, CancellationToken cancellationToken = default)
    {
        var query = await GetQueryForNavigationPropertiesAsync();
        query = ApplyFilter(query, filterText, dataOrdineMin, dataOrdineMax, stato, importoTotaleMin, importoTotaleMax, indSpedizione, metodoPagamento, clienteId, scontoId, indirizzoId);
        return await query.LongCountAsync(GetCancellationToken(cancellationToken));
    }

    protected virtual IQueryable<Ordine> ApplyFilter(IQueryable<Ordine> query, string? filterText = null, DateTime? dataOrdineMin = null, DateTime? dataOrdineMax = null, string? stato = null, decimal? importoTotaleMin = null, decimal? importoTotaleMax = null, string? indSpedizione = null, string? metodoPagamento = null)
    {
        return query.WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.Stato!.Contains(filterText!) || e.IndSpedizione!.Contains(filterText!) || e.MetodoPagamento!.Contains(filterText!)).WhereIf(dataOrdineMin.HasValue, e => e.DataOrdine >= dataOrdineMin!.Value).WhereIf(dataOrdineMax.HasValue, e => e.DataOrdine <= dataOrdineMax!.Value).WhereIf(!string.IsNullOrWhiteSpace(stato), e => e.Stato.Contains(stato)).WhereIf(importoTotaleMin.HasValue, e => e.ImportoTotale >= importoTotaleMin!.Value).WhereIf(importoTotaleMax.HasValue, e => e.ImportoTotale <= importoTotaleMax!.Value).WhereIf(!string.IsNullOrWhiteSpace(indSpedizione), e => e.IndSpedizione.Contains(indSpedizione)).WhereIf(!string.IsNullOrWhiteSpace(metodoPagamento), e => e.MetodoPagamento.Contains(metodoPagamento));
    }
}