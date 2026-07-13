using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace LFG.Ordini;

public partial interface IOrdineRepository : IRepository<Ordine, Guid>
{
    Task DeleteAllAsync(string? filterText = null, DateTime? dataOrdineMin = null, DateTime? dataOrdineMax = null, string? stato = null, decimal? importoTotaleMin = null, decimal? importoTotaleMax = null, string? indSpedizione = null, string? metodoPagamento = null, Guid? clienteId = null, Guid? scontoId = null, Guid? indirizzoId = null, CancellationToken cancellationToken = default);
    Task<OrdineWithNavigationProperties> GetWithNavigationPropertiesAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<OrdineWithNavigationProperties>> GetListWithNavigationPropertiesAsync(string? filterText = null, DateTime? dataOrdineMin = null, DateTime? dataOrdineMax = null, string? stato = null, decimal? importoTotaleMin = null, decimal? importoTotaleMax = null, string? indSpedizione = null, string? metodoPagamento = null, Guid? clienteId = null, Guid? scontoId = null, Guid? indirizzoId = null, string? sorting = null, int maxResultCount = int.MaxValue, int skipCount = 0, CancellationToken cancellationToken = default);
    Task<List<Ordine>> GetListAsync(string? filterText = null, DateTime? dataOrdineMin = null, DateTime? dataOrdineMax = null, string? stato = null, decimal? importoTotaleMin = null, decimal? importoTotaleMax = null, string? indSpedizione = null, string? metodoPagamento = null, string? sorting = null, int maxResultCount = int.MaxValue, int skipCount = 0, CancellationToken cancellationToken = default);
    Task<long> GetCountAsync(string? filterText = null, DateTime? dataOrdineMin = null, DateTime? dataOrdineMax = null, string? stato = null, decimal? importoTotaleMin = null, decimal? importoTotaleMax = null, string? indSpedizione = null, string? metodoPagamento = null, Guid? clienteId = null, Guid? scontoId = null, Guid? indirizzoId = null, CancellationToken cancellationToken = default);
}