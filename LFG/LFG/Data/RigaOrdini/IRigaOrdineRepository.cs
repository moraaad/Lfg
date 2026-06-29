using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace LFG.RigaOrdini;

public partial interface IRigaOrdineRepository : IRepository<RigaOrdine, Guid>
{
    Task DeleteAllAsync(string? filterText = null, int? quantitaMin = null, int? quantitaMax = null, decimal? prezzoUnitarioMin = null, decimal? prezzoUnitarioMax = null, Guid? ordineId = null, Guid? varianteProdottoId = null, CancellationToken cancellationToken = default);
    Task<RigaOrdineWithNavigationProperties> GetWithNavigationPropertiesAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<RigaOrdineWithNavigationProperties>> GetListWithNavigationPropertiesAsync(string? filterText = null, int? quantitaMin = null, int? quantitaMax = null, decimal? prezzoUnitarioMin = null, decimal? prezzoUnitarioMax = null, Guid? ordineId = null, Guid? varianteProdottoId = null, string? sorting = null, int maxResultCount = int.MaxValue, int skipCount = 0, CancellationToken cancellationToken = default);
    Task<List<RigaOrdine>> GetListAsync(string? filterText = null, int? quantitaMin = null, int? quantitaMax = null, decimal? prezzoUnitarioMin = null, decimal? prezzoUnitarioMax = null, string? sorting = null, int maxResultCount = int.MaxValue, int skipCount = 0, CancellationToken cancellationToken = default);
    Task<long> GetCountAsync(string? filterText = null, int? quantitaMin = null, int? quantitaMax = null, decimal? prezzoUnitarioMin = null, decimal? prezzoUnitarioMax = null, Guid? ordineId = null, Guid? varianteProdottoId = null, CancellationToken cancellationToken = default);
}