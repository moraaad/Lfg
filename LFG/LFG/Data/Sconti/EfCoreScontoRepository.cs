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

namespace LFG.Sconti;

public abstract class EfCoreScontoRepositoryBase : EfCoreRepository<LFGDbContext, Sconto, Guid>
{
    public EfCoreScontoRepositoryBase(IDbContextProvider<LFGDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public virtual async Task DeleteAllAsync(string? filterText = null, string? codice = null, string? tipo = null, decimal? valoreMin = null, decimal? valoreMax = null, int? limiteUtilizziMin = null, int? limiteUtilizziMax = null, DateTime? validoDalMin = null, DateTime? validoDalMax = null, DateTime? validoAlMin = null, DateTime? validoAlMax = null, string? sezione = null, CancellationToken cancellationToken = default)
    {
        var query = await GetQueryableAsync();
        query = ApplyFilter(query, filterText, codice, tipo, valoreMin, valoreMax, limiteUtilizziMin, limiteUtilizziMax, validoDalMin, validoDalMax, validoAlMin, validoAlMax, sezione);
        var ids = query.Select(x => x.Id);
        await DeleteManyAsync(ids, cancellationToken: GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<Sconto>> GetListAsync(string? filterText = null, string? codice = null, string? tipo = null, decimal? valoreMin = null, decimal? valoreMax = null, int? limiteUtilizziMin = null, int? limiteUtilizziMax = null, DateTime? validoDalMin = null, DateTime? validoDalMax = null, DateTime? validoAlMin = null, DateTime? validoAlMax = null, string? sezione = null, string? sorting = null, int maxResultCount = int.MaxValue, int skipCount = 0, CancellationToken cancellationToken = default)
    {
        var query = ApplyFilter((await GetQueryableAsync()), filterText, codice, tipo, valoreMin, valoreMax, limiteUtilizziMin, limiteUtilizziMax, validoDalMin, validoDalMax, validoAlMin, validoAlMax, sezione);
        query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? ScontoConsts.GetDefaultSorting(false) : sorting);
        return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
    }

    public virtual async Task<long> GetCountAsync(string? filterText = null, string? codice = null, string? tipo = null, decimal? valoreMin = null, decimal? valoreMax = null, int? limiteUtilizziMin = null, int? limiteUtilizziMax = null, DateTime? validoDalMin = null, DateTime? validoDalMax = null, DateTime? validoAlMin = null, DateTime? validoAlMax = null, string? sezione = null, CancellationToken cancellationToken = default)
    {
        var query = ApplyFilter((await GetDbSetAsync()), filterText, codice, tipo, valoreMin, valoreMax, limiteUtilizziMin, limiteUtilizziMax, validoDalMin, validoDalMax, validoAlMin, validoAlMax, sezione);
        return await query.LongCountAsync(GetCancellationToken(cancellationToken));
    }

    protected virtual IQueryable<Sconto> ApplyFilter(IQueryable<Sconto> query, string? filterText = null, string? codice = null, string? tipo = null, decimal? valoreMin = null, decimal? valoreMax = null, int? limiteUtilizziMin = null, int? limiteUtilizziMax = null, DateTime? validoDalMin = null, DateTime? validoDalMax = null, DateTime? validoAlMin = null, DateTime? validoAlMax = null, string? sezione = null)
    {
        return query.WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.Codice!.Contains(filterText!) || e.Tipo!.Contains(filterText!) || e.Sezione!.Contains(filterText!)).WhereIf(!string.IsNullOrWhiteSpace(codice), e => e.Codice.Contains(codice)).WhereIf(!string.IsNullOrWhiteSpace(tipo), e => e.Tipo.Contains(tipo)).WhereIf(valoreMin.HasValue, e => e.Valore >= valoreMin!.Value).WhereIf(valoreMax.HasValue, e => e.Valore <= valoreMax!.Value).WhereIf(limiteUtilizziMin.HasValue, e => e.LimiteUtilizzi >= limiteUtilizziMin!.Value).WhereIf(limiteUtilizziMax.HasValue, e => e.LimiteUtilizzi <= limiteUtilizziMax!.Value).WhereIf(validoDalMin.HasValue, e => e.ValidoDal >= validoDalMin!.Value).WhereIf(validoDalMax.HasValue, e => e.ValidoDal <= validoDalMax!.Value).WhereIf(validoAlMin.HasValue, e => e.ValidoAl >= validoAlMin!.Value).WhereIf(validoAlMax.HasValue, e => e.ValidoAl <= validoAlMax!.Value).WhereIf(!string.IsNullOrWhiteSpace(sezione), e => e.Sezione.Contains(sezione));
    }
}