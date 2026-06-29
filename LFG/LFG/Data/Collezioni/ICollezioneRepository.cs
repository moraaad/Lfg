using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace LFG.Collezioni;

public partial interface ICollezioneRepository : IRepository<Collezione, Guid>
{
    Task DeleteAllAsync(string? filterText = null, string? nome = null, string? stagione = null, DateTime? annoMin = null, DateTime? annoMax = null, string? sezione = null, CancellationToken cancellationToken = default);
    Task<List<Collezione>> GetListAsync(string? filterText = null, string? nome = null, string? stagione = null, DateTime? annoMin = null, DateTime? annoMax = null, string? sezione = null, string? sorting = null, int maxResultCount = int.MaxValue, int skipCount = 0, CancellationToken cancellationToken = default);
    Task<long> GetCountAsync(string? filterText = null, string? nome = null, string? stagione = null, DateTime? annoMin = null, DateTime? annoMax = null, string? sezione = null, CancellationToken cancellationToken = default);
}