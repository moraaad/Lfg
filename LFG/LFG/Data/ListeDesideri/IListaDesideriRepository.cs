using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace LFG.ListeDesideri;

public partial interface IListaDesideriRepository : IRepository<ListaDesideri, Guid>
{
    Task DeleteAllAsync(string? filterText = null, DateTime? dataCreazioneMin = null, DateTime? dataCreazioneMax = null, string? nomeLista = null, Guid? clienteId = null, CancellationToken cancellationToken = default);
    Task<ListaDesideriWithNavigationProperties> GetWithNavigationPropertiesAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<ListaDesideriWithNavigationProperties>> GetListWithNavigationPropertiesAsync(string? filterText = null, DateTime? dataCreazioneMin = null, DateTime? dataCreazioneMax = null, string? nomeLista = null, Guid? clienteId = null, string? sorting = null, int maxResultCount = int.MaxValue, int skipCount = 0, CancellationToken cancellationToken = default);
    Task<List<ListaDesideri>> GetListAsync(string? filterText = null, DateTime? dataCreazioneMin = null, DateTime? dataCreazioneMax = null, string? nomeLista = null, string? sorting = null, int maxResultCount = int.MaxValue, int skipCount = 0, CancellationToken cancellationToken = default);
    Task<long> GetCountAsync(string? filterText = null, DateTime? dataCreazioneMin = null, DateTime? dataCreazioneMax = null, string? nomeLista = null, Guid? clienteId = null, CancellationToken cancellationToken = default);
}