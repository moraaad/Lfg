using LFG.ListeDesideri;
using LFG.VarianteProdotti;
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

namespace LFG.ElementoListe;

public abstract class EfCoreElementoListaRepositoryBase : EfCoreRepository<LFGDbContext, ElementoLista, Guid>
{
    public EfCoreElementoListaRepositoryBase(IDbContextProvider<LFGDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public virtual async Task DeleteAllAsync(string? filterText = null, DateTime? dataAggiuntaMin = null, DateTime? dataAggiuntaMax = null, Guid? varianteProdottoId = null, Guid? listaDesideriId = null, CancellationToken cancellationToken = default)
    {
        var query = await GetQueryForNavigationPropertiesAsync();
        query = ApplyFilter(query, filterText, dataAggiuntaMin, dataAggiuntaMax, varianteProdottoId, listaDesideriId);
        var ids = query.Select(x => x.ElementoLista.Id);
        await DeleteManyAsync(ids, cancellationToken: GetCancellationToken(cancellationToken));
    }

    public virtual async Task<ElementoListaWithNavigationProperties> GetWithNavigationPropertiesAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        return (await GetDbSetAsync()).Where(b => b.Id == id).Select(elementoLista => new ElementoListaWithNavigationProperties { ElementoLista = elementoLista, VarianteProdotto = dbContext.Set<VarianteProdotto>().FirstOrDefault(c => c.Id == elementoLista.VarianteProdottoId), ListaDesideri = dbContext.Set<ListaDesideri>().FirstOrDefault(c => c.Id == elementoLista.ListaDesideriId) }).FirstOrDefault();
    }

    public virtual async Task<List<ElementoListaWithNavigationProperties>> GetListWithNavigationPropertiesAsync(string? filterText = null, DateTime? dataAggiuntaMin = null, DateTime? dataAggiuntaMax = null, Guid? varianteProdottoId = null, Guid? listaDesideriId = null, string? sorting = null, int maxResultCount = int.MaxValue, int skipCount = 0, CancellationToken cancellationToken = default)
    {
        var query = await GetQueryForNavigationPropertiesAsync();
        query = ApplyFilter(query, filterText, dataAggiuntaMin, dataAggiuntaMax, varianteProdottoId, listaDesideriId);
        query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? ElementoListaConsts.GetDefaultSorting(true) : sorting);
        return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
    }

    protected virtual async Task<IQueryable<ElementoListaWithNavigationProperties>> GetQueryForNavigationPropertiesAsync()
    {
        return from elementoLista in (await GetDbSetAsync())
               join varianteProdotto in (await GetDbContextAsync()).Set<VarianteProdotto>() on elementoLista.VarianteProdottoId equals varianteProdotto.Id into varianteProdottos
               from varianteProdotto in varianteProdottos.DefaultIfEmpty()
               join listaDesideri in (await GetDbContextAsync()).Set<ListaDesideri>() on elementoLista.ListaDesideriId equals listaDesideri.Id into listaDesideris
               from listaDesideri in listaDesideris.DefaultIfEmpty()
               select new ElementoListaWithNavigationProperties
               {
                   ElementoLista = elementoLista,
                   VarianteProdotto = varianteProdotto,
                   ListaDesideri = listaDesideri
               };
    }

    protected virtual IQueryable<ElementoListaWithNavigationProperties> ApplyFilter(IQueryable<ElementoListaWithNavigationProperties> query, string? filterText, DateTime? dataAggiuntaMin = null, DateTime? dataAggiuntaMax = null, Guid? varianteProdottoId = null, Guid? listaDesideriId = null)
    {
        return query.WhereIf(!string.IsNullOrWhiteSpace(filterText), e => true).WhereIf(dataAggiuntaMin.HasValue, e => e.ElementoLista.DataAggiunta >= dataAggiuntaMin!.Value).WhereIf(dataAggiuntaMax.HasValue, e => e.ElementoLista.DataAggiunta <= dataAggiuntaMax!.Value).WhereIf(varianteProdottoId != null && varianteProdottoId != Guid.Empty, e => e.VarianteProdotto != null && e.VarianteProdotto.Id == varianteProdottoId).WhereIf(listaDesideriId != null && listaDesideriId != Guid.Empty, e => e.ListaDesideri != null && e.ListaDesideri.Id == listaDesideriId);
    }

    public virtual async Task<List<ElementoLista>> GetListAsync(string? filterText = null, DateTime? dataAggiuntaMin = null, DateTime? dataAggiuntaMax = null, string? sorting = null, int maxResultCount = int.MaxValue, int skipCount = 0, CancellationToken cancellationToken = default)
    {
        var query = ApplyFilter((await GetQueryableAsync()), filterText, dataAggiuntaMin, dataAggiuntaMax);
        query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? ElementoListaConsts.GetDefaultSorting(false) : sorting);
        return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
    }

    public virtual async Task<long> GetCountAsync(string? filterText = null, DateTime? dataAggiuntaMin = null, DateTime? dataAggiuntaMax = null, Guid? varianteProdottoId = null, Guid? listaDesideriId = null, CancellationToken cancellationToken = default)
    {
        var query = await GetQueryForNavigationPropertiesAsync();
        query = ApplyFilter(query, filterText, dataAggiuntaMin, dataAggiuntaMax, varianteProdottoId, listaDesideriId);
        return await query.LongCountAsync(GetCancellationToken(cancellationToken));
    }

    protected virtual IQueryable<ElementoLista> ApplyFilter(IQueryable<ElementoLista> query, string? filterText = null, DateTime? dataAggiuntaMin = null, DateTime? dataAggiuntaMax = null)
    {
        return query.WhereIf(!string.IsNullOrWhiteSpace(filterText), e => true).WhereIf(dataAggiuntaMin.HasValue, e => e.DataAggiunta >= dataAggiuntaMin!.Value).WhereIf(dataAggiuntaMax.HasValue, e => e.DataAggiunta <= dataAggiuntaMax!.Value);
    }
}