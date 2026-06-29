using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace LFG.Indirizzi;

public partial interface IIndirizzoRepository : IRepository<Indirizzo, Guid>
{
    Task DeleteAllAsync(string? filterText = null, string? paese = null, string? citta = null, string? provincia = null, string? via = null, string? cap = null, Guid? clienteId = null, CancellationToken cancellationToken = default);
    Task<IndirizzoWithNavigationProperties> GetWithNavigationPropertiesAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<IndirizzoWithNavigationProperties>> GetListWithNavigationPropertiesAsync(string? filterText = null, string? paese = null, string? citta = null, string? provincia = null, string? via = null, string? cap = null, Guid? clienteId = null, string? sorting = null, int maxResultCount = int.MaxValue, int skipCount = 0, CancellationToken cancellationToken = default);
    Task<List<Indirizzo>> GetListAsync(string? filterText = null, string? paese = null, string? citta = null, string? provincia = null, string? via = null, string? cap = null, string? sorting = null, int maxResultCount = int.MaxValue, int skipCount = 0, CancellationToken cancellationToken = default);
    Task<long> GetCountAsync(string? filterText = null, string? paese = null, string? citta = null, string? provincia = null, string? via = null, string? cap = null, Guid? clienteId = null, CancellationToken cancellationToken = default);
}