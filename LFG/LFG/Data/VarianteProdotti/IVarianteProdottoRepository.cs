using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace LFG.VarianteProdotti;

public partial interface IVarianteProdottoRepository : IRepository<VarianteProdotto, Guid>
{
    Task<List<VarianteProdotto>> GetListByProdottoIdAsync(Guid prodottoId, string? sorting = null, int maxResultCount = int.MaxValue, int skipCount = 0, CancellationToken cancellationToken = default);
    Task<long> GetCountByProdottoIdAsync(Guid prodottoId, CancellationToken cancellationToken = default);
    Task<List<VarianteProdotto>> GetListAsync(string? filterText = null, string? taglia = null, string? colore = null, string? materiale = null, string? urlImmagine = null, int? qtaMagazzinoMin = null, int? qtaMagazzinoMax = null, string? sorting = null, int maxResultCount = int.MaxValue, int skipCount = 0, CancellationToken cancellationToken = default);
    Task<long> GetCountAsync(string? filterText = null, string? taglia = null, string? colore = null, string? materiale = null, string? urlImmagine = null, int? qtaMagazzinoMin = null, int? qtaMagazzinoMax = null, CancellationToken cancellationToken = default);
}