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

namespace LFG.VarianteProdotti;

public abstract class EfCoreVarianteProdottoRepositoryBase : EfCoreRepository<LFGDbContext, VarianteProdotto, Guid>
{
    public EfCoreVarianteProdottoRepositoryBase(IDbContextProvider<LFGDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public virtual async Task<List<VarianteProdotto>> GetListByProdottoIdAsync(Guid prodottoId, string? sorting = null, int maxResultCount = int.MaxValue, int skipCount = 0, CancellationToken cancellationToken = default)
    {
        var query = (await GetQueryableAsync()).Where(x => x.ProdottoId == prodottoId);
        query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? VarianteProdottoConsts.GetDefaultSorting(false) : sorting);
        return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
    }

    public virtual async Task<long> GetCountByProdottoIdAsync(Guid prodottoId, CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync()).Where(x => x.ProdottoId == prodottoId).CountAsync(cancellationToken);
    }

    public virtual async Task<List<VarianteProdotto>> GetListAsync(string? filterText = null, string? taglia = null, string? colore = null, string? materiale = null, string? urlImmagine = null, int? qtaMagazzinoMin = null, int? qtaMagazzinoMax = null, string? sorting = null, int maxResultCount = int.MaxValue, int skipCount = 0, CancellationToken cancellationToken = default)
    {
        var query = ApplyFilter((await GetQueryableAsync()), filterText, taglia, colore, materiale, urlImmagine, qtaMagazzinoMin, qtaMagazzinoMax);
        query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? VarianteProdottoConsts.GetDefaultSorting(false) : sorting);
        return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
    }

    public virtual async Task<long> GetCountAsync(string? filterText = null, string? taglia = null, string? colore = null, string? materiale = null, string? urlImmagine = null, int? qtaMagazzinoMin = null, int? qtaMagazzinoMax = null, CancellationToken cancellationToken = default)
    {
        var query = ApplyFilter((await GetDbSetAsync()), filterText, taglia, colore, materiale, urlImmagine, qtaMagazzinoMin, qtaMagazzinoMax);
        return await query.LongCountAsync(GetCancellationToken(cancellationToken));
    }

    protected virtual IQueryable<VarianteProdotto> ApplyFilter(IQueryable<VarianteProdotto> query, string? filterText = null, string? taglia = null, string? colore = null, string? materiale = null, string? urlImmagine = null, int? qtaMagazzinoMin = null, int? qtaMagazzinoMax = null)
    {
        return query.WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.Taglia!.Contains(filterText!) || e.Colore!.Contains(filterText!) || e.Materiale!.Contains(filterText!) || e.UrlImmagine!.Contains(filterText!)).WhereIf(!string.IsNullOrWhiteSpace(taglia), e => e.Taglia.Contains(taglia)).WhereIf(!string.IsNullOrWhiteSpace(colore), e => e.Colore.Contains(colore)).WhereIf(!string.IsNullOrWhiteSpace(materiale), e => e.Materiale.Contains(materiale)).WhereIf(!string.IsNullOrWhiteSpace(urlImmagine), e => e.UrlImmagine.Contains(urlImmagine)).WhereIf(qtaMagazzinoMin.HasValue, e => e.QtaMagazzino >= qtaMagazzinoMin!.Value).WhereIf(qtaMagazzinoMax.HasValue, e => e.QtaMagazzino <= qtaMagazzinoMax!.Value);
    }
}