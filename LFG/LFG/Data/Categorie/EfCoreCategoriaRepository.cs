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

namespace LFG.Categorie;

public abstract class EfCoreCategoriaRepositoryBase : EfCoreRepository<LFGDbContext, Categoria, Guid>
{
    public EfCoreCategoriaRepositoryBase(IDbContextProvider<LFGDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public virtual async Task DeleteAllAsync(string? filterText = null, string? nome = null, string? descrizione = null, string? sezione = null, CancellationToken cancellationToken = default)
    {
        var query = await GetQueryableAsync();
        query = ApplyFilter(query, filterText, nome, descrizione, sezione);
        var ids = query.Select(x => x.Id);
        await DeleteManyAsync(ids, cancellationToken: GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<Categoria>> GetListAsync(string? filterText = null, string? nome = null, string? descrizione = null, string? sezione = null, string? sorting = null, int maxResultCount = int.MaxValue, int skipCount = 0, CancellationToken cancellationToken = default)
    {
        var query = ApplyFilter((await GetQueryableAsync()), filterText, nome, descrizione, sezione);
        query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? CategoriaConsts.GetDefaultSorting(false) : sorting);
        return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
    }

    public virtual async Task<long> GetCountAsync(string? filterText = null, string? nome = null, string? descrizione = null, string? sezione = null, CancellationToken cancellationToken = default)
    {
        var query = ApplyFilter((await GetDbSetAsync()), filterText, nome, descrizione, sezione);
        return await query.LongCountAsync(GetCancellationToken(cancellationToken));
    }

    protected virtual IQueryable<Categoria> ApplyFilter(IQueryable<Categoria> query, string? filterText = null, string? nome = null, string? descrizione = null, string? sezione = null)
    {
        return query.WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.Nome!.Contains(filterText!) || e.Descrizione!.Contains(filterText!) || e.Sezione!.Contains(filterText!)).WhereIf(!string.IsNullOrWhiteSpace(nome), e => e.Nome.Contains(nome)).WhereIf(!string.IsNullOrWhiteSpace(descrizione), e => e.Descrizione.Contains(descrizione)).WhereIf(!string.IsNullOrWhiteSpace(sezione), e => e.Sezione.Contains(sezione));
    }
}