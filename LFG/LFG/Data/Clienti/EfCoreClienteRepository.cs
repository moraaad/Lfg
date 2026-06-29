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

namespace LFG.Clienti;

public abstract class EfCoreClienteRepositoryBase : EfCoreRepository<LFGDbContext, Cliente, Guid>
{
    public EfCoreClienteRepositoryBase(IDbContextProvider<LFGDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public virtual async Task DeleteAllAsync(string? filterText = null, string? nome = null, string? cognome = null, string? genere = null, DateTime? dataNascitaMin = null, DateTime? dataNascitaMax = null, string? email = null, string? telefono = null, string? sezione = null, string? nazionalita = null, CancellationToken cancellationToken = default)
    {
        var query = await GetQueryableAsync();
        query = ApplyFilter(query, filterText, nome, cognome, genere, dataNascitaMin, dataNascitaMax, email, telefono, sezione, nazionalita);
        var ids = query.Select(x => x.Id);
        await DeleteManyAsync(ids, cancellationToken: GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<Cliente>> GetListAsync(string? filterText = null, string? nome = null, string? cognome = null, string? genere = null, DateTime? dataNascitaMin = null, DateTime? dataNascitaMax = null, string? email = null, string? telefono = null, string? sezione = null, string? nazionalita = null, string? sorting = null, int maxResultCount = int.MaxValue, int skipCount = 0, CancellationToken cancellationToken = default)
    {
        var query = ApplyFilter((await GetQueryableAsync()), filterText, nome, cognome, genere, dataNascitaMin, dataNascitaMax, email, telefono, sezione, nazionalita);
        query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? ClienteConsts.GetDefaultSorting(false) : sorting);
        return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
    }

    public virtual async Task<long> GetCountAsync(string? filterText = null, string? nome = null, string? cognome = null, string? genere = null, DateTime? dataNascitaMin = null, DateTime? dataNascitaMax = null, string? email = null, string? telefono = null, string? sezione = null, string? nazionalita = null, CancellationToken cancellationToken = default)
    {
        var query = ApplyFilter((await GetDbSetAsync()), filterText, nome, cognome, genere, dataNascitaMin, dataNascitaMax, email, telefono, sezione, nazionalita);
        return await query.LongCountAsync(GetCancellationToken(cancellationToken));
    }

    protected virtual IQueryable<Cliente> ApplyFilter(IQueryable<Cliente> query, string? filterText = null, string? nome = null, string? cognome = null, string? genere = null, DateTime? dataNascitaMin = null, DateTime? dataNascitaMax = null, string? email = null, string? telefono = null, string? sezione = null, string? nazionalita = null)
    {
        return query.WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.Nome!.Contains(filterText!) || e.Cognome!.Contains(filterText!) || e.Genere!.Contains(filterText!) || e.Email!.Contains(filterText!) || e.Telefono!.Contains(filterText!) || e.Sezione!.Contains(filterText!) || e.Nazionalita!.Contains(filterText!)).WhereIf(!string.IsNullOrWhiteSpace(nome), e => e.Nome.Contains(nome)).WhereIf(!string.IsNullOrWhiteSpace(cognome), e => e.Cognome.Contains(cognome)).WhereIf(!string.IsNullOrWhiteSpace(genere), e => e.Genere.Contains(genere)).WhereIf(dataNascitaMin.HasValue, e => e.DataNascita >= dataNascitaMin!.Value).WhereIf(dataNascitaMax.HasValue, e => e.DataNascita <= dataNascitaMax!.Value).WhereIf(!string.IsNullOrWhiteSpace(email), e => e.Email.Contains(email)).WhereIf(!string.IsNullOrWhiteSpace(telefono), e => e.Telefono.Contains(telefono)).WhereIf(!string.IsNullOrWhiteSpace(sezione), e => e.Sezione.Contains(sezione)).WhereIf(!string.IsNullOrWhiteSpace(nazionalita), e => e.Nazionalita.Contains(nazionalita));
    }
}