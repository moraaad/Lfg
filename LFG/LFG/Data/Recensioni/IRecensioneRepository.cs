using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace LFG.Recensioni;

public partial interface IRecensioneRepository : IRepository<Recensione, Guid>
{
    Task DeleteAllAsync(string? filterText = null, int? valutazioneMin = null, int? valutazioneMax = null, string? commento = null, DateTime? dataRecensioneMin = null, DateTime? dataRecensioneMax = null, Guid? clienteId = null, Guid? prodottoId = null, CancellationToken cancellationToken = default);
    Task<RecensioneWithNavigationProperties> GetWithNavigationPropertiesAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<RecensioneWithNavigationProperties>> GetListWithNavigationPropertiesAsync(string? filterText = null, int? valutazioneMin = null, int? valutazioneMax = null, string? commento = null, DateTime? dataRecensioneMin = null, DateTime? dataRecensioneMax = null, Guid? clienteId = null, Guid? prodottoId = null, string? sorting = null, int maxResultCount = int.MaxValue, int skipCount = 0, CancellationToken cancellationToken = default);
    Task<List<Recensione>> GetListAsync(string? filterText = null, int? valutazioneMin = null, int? valutazioneMax = null, string? commento = null, DateTime? dataRecensioneMin = null, DateTime? dataRecensioneMax = null, string? sorting = null, int maxResultCount = int.MaxValue, int skipCount = 0, CancellationToken cancellationToken = default);
    Task<long> GetCountAsync(string? filterText = null, int? valutazioneMin = null, int? valutazioneMax = null, string? commento = null, DateTime? dataRecensioneMin = null, DateTime? dataRecensioneMax = null, Guid? clienteId = null, Guid? prodottoId = null, CancellationToken cancellationToken = default);
}