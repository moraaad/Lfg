using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace LFG.Clienti;

public partial interface IClienteRepository : IRepository<Cliente, Guid>
{
    Task DeleteAllAsync(string? filterText = null, string? nome = null, string? cognome = null, string? genere = null, DateTime? dataNascitaMin = null, DateTime? dataNascitaMax = null, string? email = null, string? telefono = null, string? sezione = null, string? nazionalita = null, Guid? userId = null, CancellationToken cancellationToken = default);
    Task<List<Cliente>> GetListAsync(string? filterText = null, string? nome = null, string? cognome = null, string? genere = null, DateTime? dataNascitaMin = null, DateTime? dataNascitaMax = null, string? email = null, string? telefono = null, string? sezione = null, string? nazionalita = null, Guid? userId = null, string? sorting = null, int maxResultCount = int.MaxValue, int skipCount = 0, CancellationToken cancellationToken = default);
    Task<long> GetCountAsync(string? filterText = null, string? nome = null, string? cognome = null, string? genere = null, DateTime? dataNascitaMin = null, DateTime? dataNascitaMax = null, string? email = null, string? telefono = null, string? sezione = null, string? nazionalita = null, Guid? userId = null, CancellationToken cancellationToken = default);
}