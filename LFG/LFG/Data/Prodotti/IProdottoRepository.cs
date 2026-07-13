using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace LFG.Prodotti;

public partial interface IProdottoRepository : IRepository<Prodotto, Guid>
{
    Task DeleteAllAsync(string? filterText = null, string? nome = null, string? descrizione = null, string? prezzo = null, string? codiceSku = null, string? sezione = null, Guid? categoriaId = null, Guid? collezioneId = null, CancellationToken cancellationToken = default);
    Task<ProdottoWithNavigationProperties> GetWithNavigationPropertiesAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<ProdottoWithNavigationProperties>> GetListWithNavigationPropertiesAsync(string? filterText = null, string? nome = null, string? descrizione = null, string? prezzo = null, string? codiceSku = null, string? sezione = null, Guid? categoriaId = null, Guid? collezioneId = null, string? sorting = null, int maxResultCount = int.MaxValue, int skipCount = 0, CancellationToken cancellationToken = default);
    Task<List<Prodotto>> GetListAsync(string? filterText = null, string? nome = null, string? descrizione = null, string? prezzo = null, string? codiceSku = null, string? sezione = null, string? sorting = null, int maxResultCount = int.MaxValue, int skipCount = 0, CancellationToken cancellationToken = default);
    Task<long> GetCountAsync(string? filterText = null, string? nome = null, string? descrizione = null, string? prezzo = null, string? codiceSku = null, string? sezione = null, Guid? categoriaId = null, Guid? collezioneId = null, CancellationToken cancellationToken = default);
}