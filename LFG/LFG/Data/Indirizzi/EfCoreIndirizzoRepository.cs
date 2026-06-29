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

namespace LFG.Indirizzi;

public abstract class EfCoreIndirizzoRepositoryBase : EfCoreRepository<LFGDbContext, Indirizzo, Guid>
{
    public EfCoreIndirizzoRepositoryBase(IDbContextProvider<LFGDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public virtual async Task DeleteAllAsync(string? filterText = null, string? paese = null, string? citta = null, string? provincia = null, string? via = null, string? cap = null, Guid? clienteId = null, CancellationToken cancellationToken = default)
    {
        var query = await GetQueryForNavigationPropertiesAsync();
        query = ApplyFilter(query, filterText, paese, citta, provincia, via, cap, clienteId);
        var ids = query.Select(x => x.Indirizzo.Id);
        await DeleteManyAsync(ids, cancellationToken: GetCancellationToken(cancellationToken));
    }

    public virtual async Task<IndirizzoWithNavigationProperties> GetWithNavigationPropertiesAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        return (await GetDbSetAsync()).Where(b => b.Id == id).Select(indirizzo => new IndirizzoWithNavigationProperties { Indirizzo = indirizzo, Cliente = dbContext.Set<Cliente>().FirstOrDefault(c => c.Id == indirizzo.ClienteId) }).FirstOrDefault();
    }

    public virtual async Task<List<IndirizzoWithNavigationProperties>> GetListWithNavigationPropertiesAsync(string? filterText = null, string? paese = null, string? citta = null, string? provincia = null, string? via = null, string? cap = null, Guid? clienteId = null, string? sorting = null, int maxResultCount = int.MaxValue, int skipCount = 0, CancellationToken cancellationToken = default)
    {
        var query = await GetQueryForNavigationPropertiesAsync();
        query = ApplyFilter(query, filterText, paese, citta, provincia, via, cap, clienteId);
        query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? IndirizzoConsts.GetDefaultSorting(true) : sorting);
        return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
    }

    protected virtual async Task<IQueryable<IndirizzoWithNavigationProperties>> GetQueryForNavigationPropertiesAsync()
    {
        return from indirizzo in (await GetDbSetAsync())
               join cliente in (await GetDbContextAsync()).Set<Cliente>() on indirizzo.ClienteId equals cliente.Id into clientes
               from cliente in clientes.DefaultIfEmpty()
               select new IndirizzoWithNavigationProperties
               {
                   Indirizzo = indirizzo,
                   Cliente = cliente
               };
    }

    protected virtual IQueryable<IndirizzoWithNavigationProperties> ApplyFilter(IQueryable<IndirizzoWithNavigationProperties> query, string? filterText, string? paese = null, string? citta = null, string? provincia = null, string? via = null, string? cap = null, Guid? clienteId = null)
    {
        return query.WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.Indirizzo.Paese!.Contains(filterText!) || e.Indirizzo.Citta!.Contains(filterText!) || e.Indirizzo.Provincia!.Contains(filterText!) || e.Indirizzo.Via!.Contains(filterText!) || e.Indirizzo.Cap!.Contains(filterText!)).WhereIf(!string.IsNullOrWhiteSpace(paese), e => e.Indirizzo.Paese.Contains(paese)).WhereIf(!string.IsNullOrWhiteSpace(citta), e => e.Indirizzo.Citta.Contains(citta)).WhereIf(!string.IsNullOrWhiteSpace(provincia), e => e.Indirizzo.Provincia.Contains(provincia)).WhereIf(!string.IsNullOrWhiteSpace(via), e => e.Indirizzo.Via.Contains(via)).WhereIf(!string.IsNullOrWhiteSpace(cap), e => e.Indirizzo.Cap.Contains(cap)).WhereIf(clienteId != null && clienteId != Guid.Empty, e => e.Cliente != null && e.Cliente.Id == clienteId);
    }

    public virtual async Task<List<Indirizzo>> GetListAsync(string? filterText = null, string? paese = null, string? citta = null, string? provincia = null, string? via = null, string? cap = null, string? sorting = null, int maxResultCount = int.MaxValue, int skipCount = 0, CancellationToken cancellationToken = default)
    {
        var query = ApplyFilter((await GetQueryableAsync()), filterText, paese, citta, provincia, via, cap);
        query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? IndirizzoConsts.GetDefaultSorting(false) : sorting);
        return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
    }

    public virtual async Task<long> GetCountAsync(string? filterText = null, string? paese = null, string? citta = null, string? provincia = null, string? via = null, string? cap = null, Guid? clienteId = null, CancellationToken cancellationToken = default)
    {
        var query = await GetQueryForNavigationPropertiesAsync();
        query = ApplyFilter(query, filterText, paese, citta, provincia, via, cap, clienteId);
        return await query.LongCountAsync(GetCancellationToken(cancellationToken));
    }

    protected virtual IQueryable<Indirizzo> ApplyFilter(IQueryable<Indirizzo> query, string? filterText = null, string? paese = null, string? citta = null, string? provincia = null, string? via = null, string? cap = null)
    {
        return query.WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.Paese!.Contains(filterText!) || e.Citta!.Contains(filterText!) || e.Provincia!.Contains(filterText!) || e.Via!.Contains(filterText!) || e.Cap!.Contains(filterText!)).WhereIf(!string.IsNullOrWhiteSpace(paese), e => e.Paese.Contains(paese)).WhereIf(!string.IsNullOrWhiteSpace(citta), e => e.Citta.Contains(citta)).WhereIf(!string.IsNullOrWhiteSpace(provincia), e => e.Provincia.Contains(provincia)).WhereIf(!string.IsNullOrWhiteSpace(via), e => e.Via.Contains(via)).WhereIf(!string.IsNullOrWhiteSpace(cap), e => e.Cap.Contains(cap));
    }
}