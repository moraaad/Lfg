using LFG.Categorie;
using LFG.Collezioni;
using LFG.Collezioni;
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

namespace LFG.Prodotti;

public abstract class EfCoreProdottoRepositoryBase : EfCoreRepository<LFGDbContext, Prodotto, Guid>
{
    public EfCoreProdottoRepositoryBase(IDbContextProvider<LFGDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public virtual async Task DeleteAllAsync(string? filterText = null, string? nome = null, string? descrizione = null, string? prezzo = null, string? codiceSku = null, string? sezione = null, Guid? categoriaId = null, Guid? collezionesId = null, CancellationToken cancellationToken = default)
    {
        var query = await GetQueryForNavigationPropertiesAsync();
        query = ApplyFilter(query, filterText, nome, descrizione, prezzo, codiceSku, sezione, categoriaId, collezionesId);
        var ids = query.Select(x => x.Prodotto.Id);
        await DeleteManyAsync(ids, cancellationToken: GetCancellationToken(cancellationToken));
    }

    public virtual async Task<ProdottoWithNavigationProperties> GetWithNavigationPropertiesAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        return (await GetDbSetAsync()).Where(b => b.Id == id).Include(x => x.Colleziones).Select(prodotto => new ProdottoWithNavigationProperties
        {
            Prodotto = prodotto,
            Categoria = dbContext.Set<Categoria>().FirstOrDefault(c => c.Id == prodotto.CategoriaId),
            Colleziones = (
            from prodottoColleziones in prodotto.Colleziones
            join _collezione in dbContext.Set<Collezione>() on prodottoColleziones.CollezioneId equals _collezione.Id
            select _collezione).ToList()
        }).FirstOrDefault();
    }

    public virtual async Task<List<ProdottoWithNavigationProperties>> GetListWithNavigationPropertiesAsync(string? filterText = null, string? nome = null, string? descrizione = null, string? prezzo = null, string? codiceSku = null, string? sezione = null, Guid? categoriaId = null, Guid? collezionesId = null, string? sorting = null, int maxResultCount = int.MaxValue, int skipCount = 0, CancellationToken cancellationToken = default)
    {
        var query = await GetQueryForNavigationPropertiesAsync();
        query = ApplyFilter(query, filterText, nome, descrizione, prezzo, codiceSku, sezione, categoriaId, collezionesId);
        query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? ProdottoConsts.GetDefaultSorting(true) : sorting);
        return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
    }

    protected virtual async Task<IQueryable<ProdottoWithNavigationProperties>> GetQueryForNavigationPropertiesAsync()
    {
        return from prodotto in (await GetDbSetAsync())
               join categoria in (await GetDbContextAsync()).Set<Categoria>() on prodotto.CategoriaId equals categoria.Id into categorias
               from categoria in categorias.DefaultIfEmpty()
               select new ProdottoWithNavigationProperties
               {
                   Prodotto = prodotto,
                   Categoria = categoria,
                   Colleziones = new List<Collezione>()
               };
    }

    protected virtual IQueryable<ProdottoWithNavigationProperties> ApplyFilter(IQueryable<ProdottoWithNavigationProperties> query, string? filterText, string? nome = null, string? descrizione = null, string? prezzo = null, string? codiceSku = null, string? sezione = null, Guid? categoriaId = null, Guid? collezionesId = null)
    {
        return query.WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.Prodotto.Nome!.Contains(filterText!) || e.Prodotto.Descrizione!.Contains(filterText!) || e.Prodotto.Prezzo!.Contains(filterText!) || e.Prodotto.CodiceSku!.Contains(filterText!) || e.Prodotto.Sezione!.Contains(filterText!)).WhereIf(!string.IsNullOrWhiteSpace(nome), e => e.Prodotto.Nome.Contains(nome)).WhereIf(!string.IsNullOrWhiteSpace(descrizione), e => e.Prodotto.Descrizione.Contains(descrizione)).WhereIf(!string.IsNullOrWhiteSpace(prezzo), e => e.Prodotto.Prezzo.Contains(prezzo)).WhereIf(!string.IsNullOrWhiteSpace(codiceSku), e => e.Prodotto.CodiceSku.Contains(codiceSku)).WhereIf(!string.IsNullOrWhiteSpace(sezione), e => e.Prodotto.Sezione.Contains(sezione)).WhereIf(categoriaId != null && categoriaId != Guid.Empty, e => e.Categoria != null && e.Categoria.Id == categoriaId).WhereIf(collezionesId != null && collezionesId != Guid.Empty, e => e.Prodotto.Colleziones.Any(x => x.CollezioneId == collezionesId));
    }

    public virtual async Task<List<Prodotto>> GetListAsync(string? filterText = null, string? nome = null, string? descrizione = null, string? prezzo = null, string? codiceSku = null, string? sezione = null, string? sorting = null, int maxResultCount = int.MaxValue, int skipCount = 0, CancellationToken cancellationToken = default)
    {
        var query = ApplyFilter((await GetQueryableAsync()), filterText, nome, descrizione, prezzo, codiceSku, sezione);
        query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? ProdottoConsts.GetDefaultSorting(false) : sorting);
        return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
    }

    public virtual async Task<long> GetCountAsync(string? filterText = null, string? nome = null, string? descrizione = null, string? prezzo = null, string? codiceSku = null, string? sezione = null, Guid? categoriaId = null, Guid? collezionesId = null, CancellationToken cancellationToken = default)
    {
        var query = await GetQueryForNavigationPropertiesAsync();
        query = ApplyFilter(query, filterText, nome, descrizione, prezzo, codiceSku, sezione, categoriaId, collezionesId);
        return await query.LongCountAsync(GetCancellationToken(cancellationToken));
    }

    protected virtual IQueryable<Prodotto> ApplyFilter(IQueryable<Prodotto> query, string? filterText = null, string? nome = null, string? descrizione = null, string? prezzo = null, string? codiceSku = null, string? sezione = null)
    {
        return query.WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.Nome!.Contains(filterText!) || e.Descrizione!.Contains(filterText!) || e.Prezzo!.Contains(filterText!) || e.CodiceSku!.Contains(filterText!) || e.Sezione!.Contains(filterText!)).WhereIf(!string.IsNullOrWhiteSpace(nome), e => e.Nome.Contains(nome)).WhereIf(!string.IsNullOrWhiteSpace(descrizione), e => e.Descrizione.Contains(descrizione)).WhereIf(!string.IsNullOrWhiteSpace(prezzo), e => e.Prezzo.Contains(prezzo)).WhereIf(!string.IsNullOrWhiteSpace(codiceSku), e => e.CodiceSku.Contains(codiceSku)).WhereIf(!string.IsNullOrWhiteSpace(sezione), e => e.Sezione.Contains(sezione));
    }
}