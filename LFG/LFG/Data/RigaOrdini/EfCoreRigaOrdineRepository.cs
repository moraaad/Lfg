using LFG.VarianteProdotti;
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

namespace LFG.RigaOrdini;

public abstract class EfCoreRigaOrdineRepositoryBase : EfCoreRepository<LFGDbContext, RigaOrdine, Guid>
{
    public EfCoreRigaOrdineRepositoryBase(IDbContextProvider<LFGDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public virtual async Task DeleteAllAsync(string? filterText = null, int? quantitaMin = null, int? quantitaMax = null, decimal? prezzoUnitarioMin = null, decimal? prezzoUnitarioMax = null, Guid? ordineId = null, Guid? varianteProdottoId = null, CancellationToken cancellationToken = default)
    {
        var query = await GetQueryForNavigationPropertiesAsync();
        query = ApplyFilter(query, filterText, quantitaMin, quantitaMax, prezzoUnitarioMin, prezzoUnitarioMax, ordineId, varianteProdottoId);
        var ids = query.Select(x => x.RigaOrdine.Id);
        await DeleteManyAsync(ids, cancellationToken: GetCancellationToken(cancellationToken));
    }

    public virtual async Task<RigaOrdineWithNavigationProperties> GetWithNavigationPropertiesAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        return (await GetDbSetAsync()).Where(b => b.Id == id).Select(rigaOrdine => new RigaOrdineWithNavigationProperties { RigaOrdine = rigaOrdine, Ordine = dbContext.Set<Ordine>().FirstOrDefault(c => c.Id == rigaOrdine.OrdineId), VarianteProdotto = dbContext.Set<VarianteProdotto>().FirstOrDefault(c => c.Id == rigaOrdine.VarianteProdottoId) }).FirstOrDefault();
    }

    public virtual async Task<List<RigaOrdineWithNavigationProperties>> GetListWithNavigationPropertiesAsync(string? filterText = null, int? quantitaMin = null, int? quantitaMax = null, decimal? prezzoUnitarioMin = null, decimal? prezzoUnitarioMax = null, Guid? ordineId = null, Guid? varianteProdottoId = null, string? sorting = null, int maxResultCount = int.MaxValue, int skipCount = 0, CancellationToken cancellationToken = default)
    {
        var query = await GetQueryForNavigationPropertiesAsync();
        query = ApplyFilter(query, filterText, quantitaMin, quantitaMax, prezzoUnitarioMin, prezzoUnitarioMax, ordineId, varianteProdottoId);
        query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? RigaOrdineConsts.GetDefaultSorting(true) : sorting);
        return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
    }

    protected virtual async Task<IQueryable<RigaOrdineWithNavigationProperties>> GetQueryForNavigationPropertiesAsync()
    {
        return from rigaOrdine in (await GetDbSetAsync())
               join ordine in (await GetDbContextAsync()).Set<Ordine>() on rigaOrdine.OrdineId equals ordine.Id into ordines
               from ordine in ordines.DefaultIfEmpty()
               join varianteProdotto in (await GetDbContextAsync()).Set<VarianteProdotto>() on rigaOrdine.VarianteProdottoId equals varianteProdotto.Id into varianteProdottos
               from varianteProdotto in varianteProdottos.DefaultIfEmpty()
               select new RigaOrdineWithNavigationProperties
               {
                   RigaOrdine = rigaOrdine,
                   Ordine = ordine,
                   VarianteProdotto = varianteProdotto
               };
    }

    protected virtual IQueryable<RigaOrdineWithNavigationProperties> ApplyFilter(IQueryable<RigaOrdineWithNavigationProperties> query, string? filterText, int? quantitaMin = null, int? quantitaMax = null, decimal? prezzoUnitarioMin = null, decimal? prezzoUnitarioMax = null, Guid? ordineId = null, Guid? varianteProdottoId = null)
    {
        return query.WhereIf(!string.IsNullOrWhiteSpace(filterText), e => true).WhereIf(quantitaMin.HasValue, e => e.RigaOrdine.Quantita >= quantitaMin!.Value).WhereIf(quantitaMax.HasValue, e => e.RigaOrdine.Quantita <= quantitaMax!.Value).WhereIf(prezzoUnitarioMin.HasValue, e => e.RigaOrdine.PrezzoUnitario >= prezzoUnitarioMin!.Value).WhereIf(prezzoUnitarioMax.HasValue, e => e.RigaOrdine.PrezzoUnitario <= prezzoUnitarioMax!.Value).WhereIf(ordineId != null && ordineId != Guid.Empty, e => e.Ordine != null && e.Ordine.Id == ordineId).WhereIf(varianteProdottoId != null && varianteProdottoId != Guid.Empty, e => e.VarianteProdotto != null && e.VarianteProdotto.Id == varianteProdottoId);
    }

    public virtual async Task<List<RigaOrdine>> GetListAsync(string? filterText = null, int? quantitaMin = null, int? quantitaMax = null, decimal? prezzoUnitarioMin = null, decimal? prezzoUnitarioMax = null, string? sorting = null, int maxResultCount = int.MaxValue, int skipCount = 0, CancellationToken cancellationToken = default)
    {
        var query = ApplyFilter((await GetQueryableAsync()), filterText, quantitaMin, quantitaMax, prezzoUnitarioMin, prezzoUnitarioMax);
        query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? RigaOrdineConsts.GetDefaultSorting(false) : sorting);
        return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
    }

    public virtual async Task<long> GetCountAsync(string? filterText = null, int? quantitaMin = null, int? quantitaMax = null, decimal? prezzoUnitarioMin = null, decimal? prezzoUnitarioMax = null, Guid? ordineId = null, Guid? varianteProdottoId = null, CancellationToken cancellationToken = default)
    {
        var query = await GetQueryForNavigationPropertiesAsync();
        query = ApplyFilter(query, filterText, quantitaMin, quantitaMax, prezzoUnitarioMin, prezzoUnitarioMax, ordineId, varianteProdottoId);
        return await query.LongCountAsync(GetCancellationToken(cancellationToken));
    }

    protected virtual IQueryable<RigaOrdine> ApplyFilter(IQueryable<RigaOrdine> query, string? filterText = null, int? quantitaMin = null, int? quantitaMax = null, decimal? prezzoUnitarioMin = null, decimal? prezzoUnitarioMax = null)
    {
        return query.WhereIf(!string.IsNullOrWhiteSpace(filterText), e => true).WhereIf(quantitaMin.HasValue, e => e.Quantita >= quantitaMin!.Value).WhereIf(quantitaMax.HasValue, e => e.Quantita <= quantitaMax!.Value).WhereIf(prezzoUnitarioMin.HasValue, e => e.PrezzoUnitario >= prezzoUnitarioMin!.Value).WhereIf(prezzoUnitarioMax.HasValue, e => e.PrezzoUnitario <= prezzoUnitarioMax!.Value);
    }
}