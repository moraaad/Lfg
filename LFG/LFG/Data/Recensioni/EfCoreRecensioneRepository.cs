using LFG.Prodotti;
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

namespace LFG.Recensioni;

public abstract class EfCoreRecensioneRepositoryBase : EfCoreRepository<LFGDbContext, Recensione, Guid>
{
    public EfCoreRecensioneRepositoryBase(IDbContextProvider<LFGDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public virtual async Task DeleteAllAsync(string? filterText = null, int? valutazioneMin = null, int? valutazioneMax = null, string? commento = null, DateTime? dataRecensioneMin = null, DateTime? dataRecensioneMax = null, Guid? clienteId = null, Guid? prodottoId = null, CancellationToken cancellationToken = default)
    {
        var query = await GetQueryForNavigationPropertiesAsync();
        query = ApplyFilter(query, filterText, valutazioneMin, valutazioneMax, commento, dataRecensioneMin, dataRecensioneMax, clienteId, prodottoId);
        var ids = query.Select(x => x.Recensione.Id);
        await DeleteManyAsync(ids, cancellationToken: GetCancellationToken(cancellationToken));
    }

    public virtual async Task<RecensioneWithNavigationProperties> GetWithNavigationPropertiesAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        return (await GetDbSetAsync()).Where(b => b.Id == id).Select(recensione => new RecensioneWithNavigationProperties { Recensione = recensione, Cliente = dbContext.Set<Cliente>().FirstOrDefault(c => c.Id == recensione.ClienteId), Prodotto = dbContext.Set<Prodotto>().FirstOrDefault(c => c.Id == recensione.ProdottoId) }).FirstOrDefault();
    }

    public virtual async Task<List<RecensioneWithNavigationProperties>> GetListWithNavigationPropertiesAsync(string? filterText = null, int? valutazioneMin = null, int? valutazioneMax = null, string? commento = null, DateTime? dataRecensioneMin = null, DateTime? dataRecensioneMax = null, Guid? clienteId = null, Guid? prodottoId = null, string? sorting = null, int maxResultCount = int.MaxValue, int skipCount = 0, CancellationToken cancellationToken = default)
    {
        var query = await GetQueryForNavigationPropertiesAsync();
        query = ApplyFilter(query, filterText, valutazioneMin, valutazioneMax, commento, dataRecensioneMin, dataRecensioneMax, clienteId, prodottoId);
        query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? RecensioneConsts.GetDefaultSorting(true) : sorting);
        return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
    }

    protected virtual async Task<IQueryable<RecensioneWithNavigationProperties>> GetQueryForNavigationPropertiesAsync()
    {
        return from recensione in (await GetDbSetAsync())
               join cliente in (await GetDbContextAsync()).Set<Cliente>() on recensione.ClienteId equals cliente.Id into clientes
               from cliente in clientes.DefaultIfEmpty()
               join prodotto in (await GetDbContextAsync()).Set<Prodotto>() on recensione.ProdottoId equals prodotto.Id into prodottos
               from prodotto in prodottos.DefaultIfEmpty()
               select new RecensioneWithNavigationProperties
               {
                   Recensione = recensione,
                   Cliente = cliente,
                   Prodotto = prodotto
               };
    }

    protected virtual IQueryable<RecensioneWithNavigationProperties> ApplyFilter(IQueryable<RecensioneWithNavigationProperties> query, string? filterText, int? valutazioneMin = null, int? valutazioneMax = null, string? commento = null, DateTime? dataRecensioneMin = null, DateTime? dataRecensioneMax = null, Guid? clienteId = null, Guid? prodottoId = null)
    {
        return query.WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.Recensione.Commento!.Contains(filterText!)).WhereIf(valutazioneMin.HasValue, e => e.Recensione.Valutazione >= valutazioneMin!.Value).WhereIf(valutazioneMax.HasValue, e => e.Recensione.Valutazione <= valutazioneMax!.Value).WhereIf(!string.IsNullOrWhiteSpace(commento), e => e.Recensione.Commento.Contains(commento)).WhereIf(dataRecensioneMin.HasValue, e => e.Recensione.DataRecensione >= dataRecensioneMin!.Value).WhereIf(dataRecensioneMax.HasValue, e => e.Recensione.DataRecensione <= dataRecensioneMax!.Value).WhereIf(clienteId != null && clienteId != Guid.Empty, e => e.Cliente != null && e.Cliente.Id == clienteId).WhereIf(prodottoId != null && prodottoId != Guid.Empty, e => e.Prodotto != null && e.Prodotto.Id == prodottoId);
    }

    public virtual async Task<List<Recensione>> GetListAsync(string? filterText = null, int? valutazioneMin = null, int? valutazioneMax = null, string? commento = null, DateTime? dataRecensioneMin = null, DateTime? dataRecensioneMax = null, string? sorting = null, int maxResultCount = int.MaxValue, int skipCount = 0, CancellationToken cancellationToken = default)
    {
        var query = ApplyFilter((await GetQueryableAsync()), filterText, valutazioneMin, valutazioneMax, commento, dataRecensioneMin, dataRecensioneMax);
        query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? RecensioneConsts.GetDefaultSorting(false) : sorting);
        return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
    }

    public virtual async Task<long> GetCountAsync(string? filterText = null, int? valutazioneMin = null, int? valutazioneMax = null, string? commento = null, DateTime? dataRecensioneMin = null, DateTime? dataRecensioneMax = null, Guid? clienteId = null, Guid? prodottoId = null, CancellationToken cancellationToken = default)
    {
        var query = await GetQueryForNavigationPropertiesAsync();
        query = ApplyFilter(query, filterText, valutazioneMin, valutazioneMax, commento, dataRecensioneMin, dataRecensioneMax, clienteId, prodottoId);
        return await query.LongCountAsync(GetCancellationToken(cancellationToken));
    }

    protected virtual IQueryable<Recensione> ApplyFilter(IQueryable<Recensione> query, string? filterText = null, int? valutazioneMin = null, int? valutazioneMax = null, string? commento = null, DateTime? dataRecensioneMin = null, DateTime? dataRecensioneMax = null)
    {
        return query.WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.Commento!.Contains(filterText!)).WhereIf(valutazioneMin.HasValue, e => e.Valutazione >= valutazioneMin!.Value).WhereIf(valutazioneMax.HasValue, e => e.Valutazione <= valutazioneMax!.Value).WhereIf(!string.IsNullOrWhiteSpace(commento), e => e.Commento.Contains(commento)).WhereIf(dataRecensioneMin.HasValue, e => e.DataRecensione >= dataRecensioneMin!.Value).WhereIf(dataRecensioneMax.HasValue, e => e.DataRecensione <= dataRecensioneMax!.Value);
    }
}