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

namespace LFG.ImmagineVarianti;

public abstract class EfCoreImmagineVarianteRepositoryBase : EfCoreRepository<LFGDbContext, ImmagineVariante, Guid>
{
    public EfCoreImmagineVarianteRepositoryBase(IDbContextProvider<LFGDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public virtual async Task DeleteAllAsync(string? filterText = null, Guid? varianteProdottoId = null, string? url = null, int? ordineMin = null, int? ordineMax = null, CancellationToken cancellationToken = default)
    {
        var query = await GetQueryForNavigationPropertiesAsync();
        query = ApplyFilter(query, filterText, varianteProdottoId, url, ordineMin, ordineMax);
        var ids = query.Select(x => x.ImmagineVariante.Id);
        await DeleteManyAsync(ids, cancellationToken: GetCancellationToken(cancellationToken));
    }

    public virtual async Task<ImmagineVarianteWithNavigationProperties> GetWithNavigationPropertiesAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        return (await GetDbSetAsync()).Where(b => b.Id == id).Select(immagineVariante => new ImmagineVarianteWithNavigationProperties { ImmagineVariante = immagineVariante, VarianteProdotto = dbContext.Set<VarianteProdotto>().FirstOrDefault(c => c.Id == immagineVariante.VarianteProdottoId) }).FirstOrDefault();
    }

    public virtual async Task<List<ImmagineVarianteWithNavigationProperties>> GetListWithNavigationPropertiesAsync(string? filterText = null, Guid? varianteProdottoId = null, string? url = null, int? ordineMin = null, int? ordineMax = null, string? sorting = null, int maxResultCount = int.MaxValue, int skipCount = 0, CancellationToken cancellationToken = default)
    {
        var query = await GetQueryForNavigationPropertiesAsync();
        query = ApplyFilter(query, filterText, varianteProdottoId, url, ordineMin, ordineMax);
        query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? ImmagineVarianteConsts.GetDefaultSorting(true) : sorting);
        return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
    }

    protected virtual async Task<IQueryable<ImmagineVarianteWithNavigationProperties>> GetQueryForNavigationPropertiesAsync()
    {
        return from immagineVariante in (await GetDbSetAsync())
               join varianteProdotto in (await GetDbContextAsync()).Set<VarianteProdotto>() on immagineVariante.VarianteProdottoId equals varianteProdotto.Id into varianteProdottos
               from varianteProdotto in varianteProdottos.DefaultIfEmpty()
               select new ImmagineVarianteWithNavigationProperties
               {
                   ImmagineVariante = immagineVariante,
                   VarianteProdotto = varianteProdotto
               };
    }

    protected virtual IQueryable<ImmagineVarianteWithNavigationProperties> ApplyFilter(IQueryable<ImmagineVarianteWithNavigationProperties> query, string? filterText, Guid? varianteProdottoId = null, string? url = null, int? ordineMin = null, int? ordineMax = null)
    {
        return query
            .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.ImmagineVariante.Url!.Contains(filterText!))
            .WhereIf(varianteProdottoId.HasValue, e => e.ImmagineVariante.VarianteProdottoId == varianteProdottoId)
            .WhereIf(!string.IsNullOrWhiteSpace(url), e => e.ImmagineVariante.Url.Contains(url))
            .WhereIf(ordineMin.HasValue, e => e.ImmagineVariante.Ordine >= ordineMin!.Value)
            .WhereIf(ordineMax.HasValue, e => e.ImmagineVariante.Ordine <= ordineMax!.Value);
    }

    public virtual async Task<List<ImmagineVariante>> GetListAsync(string? filterText = null, Guid? varianteProdottoId = null, string? url = null, int? ordineMin = null, int? ordineMax = null, string? sorting = null, int maxResultCount = int.MaxValue, int skipCount = 0, CancellationToken cancellationToken = default)
    {
        var query = ApplyFilter((await GetQueryableAsync()), filterText, varianteProdottoId, url, ordineMin, ordineMax);
        query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? ImmagineVarianteConsts.GetDefaultSorting(false) : sorting);
        return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
    }

    public virtual async Task<long> GetCountAsync(string? filterText = null, Guid? varianteProdottoId = null, string? url = null, int? ordineMin = null, int? ordineMax = null, CancellationToken cancellationToken = default)
    {
        var query = await GetQueryForNavigationPropertiesAsync();
        query = ApplyFilter(query, filterText, varianteProdottoId, url, ordineMin, ordineMax);
        return await query.LongCountAsync(GetCancellationToken(cancellationToken));
    }

    protected virtual IQueryable<ImmagineVariante> ApplyFilter(IQueryable<ImmagineVariante> query, string? filterText = null, Guid? varianteProdottoId = null, string? url = null, int? ordineMin = null, int? ordineMax = null)
    {
        return query
            .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.Url!.Contains(filterText!))
            .WhereIf(varianteProdottoId.HasValue, e => e.VarianteProdottoId == varianteProdottoId)
            .WhereIf(!string.IsNullOrWhiteSpace(url), e => e.Url.Contains(url))
            .WhereIf(ordineMin.HasValue, e => e.Ordine >= ordineMin!.Value)
            .WhereIf(ordineMax.HasValue, e => e.Ordine <= ordineMax!.Value);
    }
}
