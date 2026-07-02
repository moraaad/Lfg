using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace LFG.ImmagineVarianti;

public partial interface IImmagineVarianteRepository : IRepository<ImmagineVariante, Guid>
{
    Task DeleteAllAsync(string? filterText = null, Guid? varianteProdottoId = null, string? url = null, int? ordineMin = null, int? ordineMax = null, CancellationToken cancellationToken = default);
    Task<ImmagineVarianteWithNavigationProperties> GetWithNavigationPropertiesAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<ImmagineVarianteWithNavigationProperties>> GetListWithNavigationPropertiesAsync(string? filterText = null, Guid? varianteProdottoId = null, string? url = null, int? ordineMin = null, int? ordineMax = null, string? sorting = null, int maxResultCount = int.MaxValue, int skipCount = 0, CancellationToken cancellationToken = default);
    Task<List<ImmagineVariante>> GetListAsync(string? filterText = null, Guid? varianteProdottoId = null, string? url = null, int? ordineMin = null, int? ordineMax = null, string? sorting = null, int maxResultCount = int.MaxValue, int skipCount = 0, CancellationToken cancellationToken = default);
    Task<long> GetCountAsync(string? filterText = null, Guid? varianteProdottoId = null, string? url = null, int? ordineMin = null, int? ordineMax = null, CancellationToken cancellationToken = default);
}
