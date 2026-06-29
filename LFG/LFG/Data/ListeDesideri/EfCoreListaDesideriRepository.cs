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

namespace LFG.ListeDesideri;

public abstract class EfCoreListaDesideriRepositoryBase : EfCoreRepository<LFGDbContext, ListaDesideri, Guid>
{
    public EfCoreListaDesideriRepositoryBase(IDbContextProvider<LFGDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public virtual async Task DeleteAllAsync(string? filterText = null, DateTime? dataCreazioneMin = null, DateTime? dataCreazioneMax = null, string? nomeLista = null, Guid? clienteId = null, CancellationToken cancellationToken = default)
    {
        var query = await GetQueryForNavigationPropertiesAsync();
        query = ApplyFilter(query, filterText, dataCreazioneMin, dataCreazioneMax, nomeLista, clienteId);
        var ids = query.Select(x => x.ListaDesideri.Id);
        await DeleteManyAsync(ids, cancellationToken: GetCancellationToken(cancellationToken));
    }

    public virtual async Task<ListaDesideriWithNavigationProperties> GetWithNavigationPropertiesAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        return (await GetDbSetAsync()).Where(b => b.Id == id).Select(listaDesideri => new ListaDesideriWithNavigationProperties { ListaDesideri = listaDesideri, Cliente = dbContext.Set<Cliente>().FirstOrDefault(c => c.Id == listaDesideri.ClienteId) }).FirstOrDefault();
    }

    public virtual async Task<List<ListaDesideriWithNavigationProperties>> GetListWithNavigationPropertiesAsync(string? filterText = null, DateTime? dataCreazioneMin = null, DateTime? dataCreazioneMax = null, string? nomeLista = null, Guid? clienteId = null, string? sorting = null, int maxResultCount = int.MaxValue, int skipCount = 0, CancellationToken cancellationToken = default)
    {
        var query = await GetQueryForNavigationPropertiesAsync();
        query = ApplyFilter(query, filterText, dataCreazioneMin, dataCreazioneMax, nomeLista, clienteId);
        query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? ListaDesideriConsts.GetDefaultSorting(true) : sorting);
        return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
    }

    protected virtual async Task<IQueryable<ListaDesideriWithNavigationProperties>> GetQueryForNavigationPropertiesAsync()
    {
        return from listaDesideri in (await GetDbSetAsync())
               join cliente in (await GetDbContextAsync()).Set<Cliente>() on listaDesideri.ClienteId equals cliente.Id into clientes
               from cliente in clientes.DefaultIfEmpty()
               select new ListaDesideriWithNavigationProperties
               {
                   ListaDesideri = listaDesideri,
                   Cliente = cliente
               };
    }

    protected virtual IQueryable<ListaDesideriWithNavigationProperties> ApplyFilter(IQueryable<ListaDesideriWithNavigationProperties> query, string? filterText, DateTime? dataCreazioneMin = null, DateTime? dataCreazioneMax = null, string? nomeLista = null, Guid? clienteId = null)
    {
        return query.WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.ListaDesideri.NomeLista!.Contains(filterText!)).WhereIf(dataCreazioneMin.HasValue, e => e.ListaDesideri.DataCreazione >= dataCreazioneMin!.Value).WhereIf(dataCreazioneMax.HasValue, e => e.ListaDesideri.DataCreazione <= dataCreazioneMax!.Value).WhereIf(!string.IsNullOrWhiteSpace(nomeLista), e => e.ListaDesideri.NomeLista.Contains(nomeLista)).WhereIf(clienteId != null && clienteId != Guid.Empty, e => e.Cliente != null && e.Cliente.Id == clienteId);
    }

    public virtual async Task<List<ListaDesideri>> GetListAsync(string? filterText = null, DateTime? dataCreazioneMin = null, DateTime? dataCreazioneMax = null, string? nomeLista = null, string? sorting = null, int maxResultCount = int.MaxValue, int skipCount = 0, CancellationToken cancellationToken = default)
    {
        var query = ApplyFilter((await GetQueryableAsync()), filterText, dataCreazioneMin, dataCreazioneMax, nomeLista);
        query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? ListaDesideriConsts.GetDefaultSorting(false) : sorting);
        return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
    }

    public virtual async Task<long> GetCountAsync(string? filterText = null, DateTime? dataCreazioneMin = null, DateTime? dataCreazioneMax = null, string? nomeLista = null, Guid? clienteId = null, CancellationToken cancellationToken = default)
    {
        var query = await GetQueryForNavigationPropertiesAsync();
        query = ApplyFilter(query, filterText, dataCreazioneMin, dataCreazioneMax, nomeLista, clienteId);
        return await query.LongCountAsync(GetCancellationToken(cancellationToken));
    }

    protected virtual IQueryable<ListaDesideri> ApplyFilter(IQueryable<ListaDesideri> query, string? filterText = null, DateTime? dataCreazioneMin = null, DateTime? dataCreazioneMax = null, string? nomeLista = null)
    {
        return query.WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.NomeLista!.Contains(filterText!)).WhereIf(dataCreazioneMin.HasValue, e => e.DataCreazione >= dataCreazioneMin!.Value).WhereIf(dataCreazioneMax.HasValue, e => e.DataCreazione <= dataCreazioneMax!.Value).WhereIf(!string.IsNullOrWhiteSpace(nomeLista), e => e.NomeLista.Contains(nomeLista));
    }
}