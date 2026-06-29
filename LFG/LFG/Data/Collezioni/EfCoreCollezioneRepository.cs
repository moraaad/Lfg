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

namespace LFG.Collezioni;

public abstract class EfCoreCollezioneRepositoryBase : EfCoreRepository<LFGDbContext, Collezione, Guid>
{
    public EfCoreCollezioneRepositoryBase(IDbContextProvider<LFGDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public virtual async Task DeleteAllAsync(string? filterText = null, string? nome = null, string? stagione = null, DateTime? annoMin = null, DateTime? annoMax = null, string? sezione = null, CancellationToken cancellationToken = default)
    {
        var query = await GetQueryableAsync();
        query = ApplyFilter(query, filterText, nome, stagione, annoMin, annoMax, sezione);
        var ids = query.Select(x => x.Id);
        await DeleteManyAsync(ids, cancellationToken: GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<Collezione>> GetListAsync(string? filterText = null, string? nome = null, string? stagione = null, DateTime? annoMin = null, DateTime? annoMax = null, string? sezione = null, string? sorting = null, int maxResultCount = int.MaxValue, int skipCount = 0, CancellationToken cancellationToken = default)
    {
        var query = ApplyFilter((await GetQueryableAsync()), filterText, nome, stagione, annoMin, annoMax, sezione);
        query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? CollezioneConsts.GetDefaultSorting(false) : sorting);
        return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
    }

    public virtual async Task<long> GetCountAsync(string? filterText = null, string? nome = null, string? stagione = null, DateTime? annoMin = null, DateTime? annoMax = null, string? sezione = null, CancellationToken cancellationToken = default)
    {
        var query = ApplyFilter((await GetDbSetAsync()), filterText, nome, stagione, annoMin, annoMax, sezione);
        return await query.LongCountAsync(GetCancellationToken(cancellationToken));
    }

    protected virtual IQueryable<Collezione> ApplyFilter(IQueryable<Collezione> query, string? filterText = null, string? nome = null, string? stagione = null, DateTime? annoMin = null, DateTime? annoMax = null, string? sezione = null)
    {
        return query.WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.Nome!.Contains(filterText!) || e.Stagione!.Contains(filterText!) || e.Sezione!.Contains(filterText!)).WhereIf(!string.IsNullOrWhiteSpace(nome), e => e.Nome.Contains(nome)).WhereIf(!string.IsNullOrWhiteSpace(stagione), e => e.Stagione.Contains(stagione)).WhereIf(annoMin.HasValue, e => e.Anno >= annoMin!.Value).WhereIf(annoMax.HasValue, e => e.Anno <= annoMax!.Value).WhereIf(!string.IsNullOrWhiteSpace(sezione), e => e.Sezione.Contains(sezione));
    }
}