using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace LFG.Pagamenti;

public partial interface IPagamentoRepository : IRepository<Pagamento, Guid>
{
    Task DeleteAllAsync(string? filterText = null, string? metodo = null, string? stato = null, decimal? importoMin = null, decimal? importoMax = null, DateTime? dataPagamentoMin = null, DateTime? dataPagamentoMax = null, string? idTransazione = null, Guid? ordineId = null, CancellationToken cancellationToken = default);
    Task<PagamentoWithNavigationProperties> GetWithNavigationPropertiesAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<PagamentoWithNavigationProperties>> GetListWithNavigationPropertiesAsync(string? filterText = null, string? metodo = null, string? stato = null, decimal? importoMin = null, decimal? importoMax = null, DateTime? dataPagamentoMin = null, DateTime? dataPagamentoMax = null, string? idTransazione = null, Guid? ordineId = null, string? sorting = null, int maxResultCount = int.MaxValue, int skipCount = 0, CancellationToken cancellationToken = default);
    Task<List<Pagamento>> GetListAsync(string? filterText = null, string? metodo = null, string? stato = null, decimal? importoMin = null, decimal? importoMax = null, DateTime? dataPagamentoMin = null, DateTime? dataPagamentoMax = null, string? idTransazione = null, string? sorting = null, int maxResultCount = int.MaxValue, int skipCount = 0, CancellationToken cancellationToken = default);
    Task<long> GetCountAsync(string? filterText = null, string? metodo = null, string? stato = null, decimal? importoMin = null, decimal? importoMax = null, DateTime? dataPagamentoMin = null, DateTime? dataPagamentoMax = null, string? idTransazione = null, Guid? ordineId = null, CancellationToken cancellationToken = default);
}