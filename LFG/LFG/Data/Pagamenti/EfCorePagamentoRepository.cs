using LFG.Ordini;
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

namespace LFG.Pagamenti;

public abstract class EfCorePagamentoRepositoryBase : EfCoreRepository<LFGDbContext, Pagamento, Guid>
{
    public EfCorePagamentoRepositoryBase(IDbContextProvider<LFGDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public virtual async Task DeleteAllAsync(string? filterText = null, string? metodo = null, string? stato = null, decimal? importoMin = null, decimal? importoMax = null, DateTime? dataPagamentoMin = null, DateTime? dataPagamentoMax = null, string? idTransazione = null, Guid? ordineId = null, CancellationToken cancellationToken = default)
    {
        var query = await GetQueryForNavigationPropertiesAsync();
        query = ApplyFilter(query, filterText, metodo, stato, importoMin, importoMax, dataPagamentoMin, dataPagamentoMax, idTransazione, ordineId);
        var ids = query.Select(x => x.Pagamento.Id);
        await DeleteManyAsync(ids, cancellationToken: GetCancellationToken(cancellationToken));
    }

    public virtual async Task<PagamentoWithNavigationProperties> GetWithNavigationPropertiesAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        return (await GetDbSetAsync()).Where(b => b.Id == id).Select(pagamento => new PagamentoWithNavigationProperties { Pagamento = pagamento, Ordine = dbContext.Set<Ordine>().FirstOrDefault(c => c.Id == pagamento.OrdineId) }).FirstOrDefault();
    }

    public virtual async Task<List<PagamentoWithNavigationProperties>> GetListWithNavigationPropertiesAsync(string? filterText = null, string? metodo = null, string? stato = null, decimal? importoMin = null, decimal? importoMax = null, DateTime? dataPagamentoMin = null, DateTime? dataPagamentoMax = null, string? idTransazione = null, Guid? ordineId = null, string? sorting = null, int maxResultCount = int.MaxValue, int skipCount = 0, CancellationToken cancellationToken = default)
    {
        var query = await GetQueryForNavigationPropertiesAsync();
        query = ApplyFilter(query, filterText, metodo, stato, importoMin, importoMax, dataPagamentoMin, dataPagamentoMax, idTransazione, ordineId);
        query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? PagamentoConsts.GetDefaultSorting(true) : sorting);
        return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
    }

    protected virtual async Task<IQueryable<PagamentoWithNavigationProperties>> GetQueryForNavigationPropertiesAsync()
    {
        return from pagamento in (await GetDbSetAsync())
               join ordine in (await GetDbContextAsync()).Set<Ordine>() on pagamento.OrdineId equals ordine.Id into ordines
               from ordine in ordines.DefaultIfEmpty()
               select new PagamentoWithNavigationProperties
               {
                   Pagamento = pagamento,
                   Ordine = ordine
               };
    }

    protected virtual IQueryable<PagamentoWithNavigationProperties> ApplyFilter(IQueryable<PagamentoWithNavigationProperties> query, string? filterText, string? metodo = null, string? stato = null, decimal? importoMin = null, decimal? importoMax = null, DateTime? dataPagamentoMin = null, DateTime? dataPagamentoMax = null, string? idTransazione = null, Guid? ordineId = null)
    {
        return query.WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.Pagamento.Metodo!.Contains(filterText!) || e.Pagamento.Stato!.Contains(filterText!) || e.Pagamento.IdTransazione!.Contains(filterText!)).WhereIf(!string.IsNullOrWhiteSpace(metodo), e => e.Pagamento.Metodo.Contains(metodo)).WhereIf(!string.IsNullOrWhiteSpace(stato), e => e.Pagamento.Stato.Contains(stato)).WhereIf(importoMin.HasValue, e => e.Pagamento.Importo >= importoMin!.Value).WhereIf(importoMax.HasValue, e => e.Pagamento.Importo <= importoMax!.Value).WhereIf(dataPagamentoMin.HasValue, e => e.Pagamento.DataPagamento >= dataPagamentoMin!.Value).WhereIf(dataPagamentoMax.HasValue, e => e.Pagamento.DataPagamento <= dataPagamentoMax!.Value).WhereIf(!string.IsNullOrWhiteSpace(idTransazione), e => e.Pagamento.IdTransazione.Contains(idTransazione)).WhereIf(ordineId != null && ordineId != Guid.Empty, e => e.Ordine != null && e.Ordine.Id == ordineId);
    }

    public virtual async Task<List<Pagamento>> GetListAsync(string? filterText = null, string? metodo = null, string? stato = null, decimal? importoMin = null, decimal? importoMax = null, DateTime? dataPagamentoMin = null, DateTime? dataPagamentoMax = null, string? idTransazione = null, string? sorting = null, int maxResultCount = int.MaxValue, int skipCount = 0, CancellationToken cancellationToken = default)
    {
        var query = ApplyFilter((await GetQueryableAsync()), filterText, metodo, stato, importoMin, importoMax, dataPagamentoMin, dataPagamentoMax, idTransazione);
        query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? PagamentoConsts.GetDefaultSorting(false) : sorting);
        return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
    }

    public virtual async Task<long> GetCountAsync(string? filterText = null, string? metodo = null, string? stato = null, decimal? importoMin = null, decimal? importoMax = null, DateTime? dataPagamentoMin = null, DateTime? dataPagamentoMax = null, string? idTransazione = null, Guid? ordineId = null, CancellationToken cancellationToken = default)
    {
        var query = await GetQueryForNavigationPropertiesAsync();
        query = ApplyFilter(query, filterText, metodo, stato, importoMin, importoMax, dataPagamentoMin, dataPagamentoMax, idTransazione, ordineId);
        return await query.LongCountAsync(GetCancellationToken(cancellationToken));
    }

    protected virtual IQueryable<Pagamento> ApplyFilter(IQueryable<Pagamento> query, string? filterText = null, string? metodo = null, string? stato = null, decimal? importoMin = null, decimal? importoMax = null, DateTime? dataPagamentoMin = null, DateTime? dataPagamentoMax = null, string? idTransazione = null)
    {
        return query.WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.Metodo!.Contains(filterText!) || e.Stato!.Contains(filterText!) || e.IdTransazione!.Contains(filterText!)).WhereIf(!string.IsNullOrWhiteSpace(metodo), e => e.Metodo.Contains(metodo)).WhereIf(!string.IsNullOrWhiteSpace(stato), e => e.Stato.Contains(stato)).WhereIf(importoMin.HasValue, e => e.Importo >= importoMin!.Value).WhereIf(importoMax.HasValue, e => e.Importo <= importoMax!.Value).WhereIf(dataPagamentoMin.HasValue, e => e.DataPagamento >= dataPagamentoMin!.Value).WhereIf(dataPagamentoMax.HasValue, e => e.DataPagamento <= dataPagamentoMax!.Value).WhereIf(!string.IsNullOrWhiteSpace(idTransazione), e => e.IdTransazione.Contains(idTransazione));
    }
}