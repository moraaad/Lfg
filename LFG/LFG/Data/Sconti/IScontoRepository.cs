using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace LFG.Sconti;

public partial interface IScontoRepository : IRepository<Sconto, Guid>
{
    Task DeleteAllAsync(string? filterText = null, string? codice = null, string? tipo = null, decimal? valoreMin = null, decimal? valoreMax = null, int? limiteUtilizziMin = null, int? limiteUtilizziMax = null, DateTime? validoDalMin = null, DateTime? validoDalMax = null, DateTime? validoAlMin = null, DateTime? validoAlMax = null, CancellationToken cancellationToken = default);
    Task<List<Sconto>> GetListAsync(string? filterText = null, string? codice = null, string? tipo = null, decimal? valoreMin = null, decimal? valoreMax = null, int? limiteUtilizziMin = null, int? limiteUtilizziMax = null, DateTime? validoDalMin = null, DateTime? validoDalMax = null, DateTime? validoAlMin = null, DateTime? validoAlMax = null, string? sorting = null, int maxResultCount = int.MaxValue, int skipCount = 0, CancellationToken cancellationToken = default);
    Task<long> GetCountAsync(string? filterText = null, string? codice = null, string? tipo = null, decimal? valoreMin = null, decimal? valoreMax = null, int? limiteUtilizziMin = null, int? limiteUtilizziMax = null, DateTime? validoDalMin = null, DateTime? validoDalMax = null, DateTime? validoAlMin = null, DateTime? validoAlMax = null, CancellationToken cancellationToken = default);
}