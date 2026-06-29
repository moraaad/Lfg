using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace LFG.ElementoListe;

public partial interface IElementoListaRepository : IRepository<ElementoLista, Guid>
{
    Task DeleteAllAsync(string? filterText = null, DateTime? dataAggiuntaMin = null, DateTime? dataAggiuntaMax = null, Guid? varianteProdottoId = null, Guid? listaDesideriId = null, CancellationToken cancellationToken = default);
    Task<ElementoListaWithNavigationProperties> GetWithNavigationPropertiesAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<ElementoListaWithNavigationProperties>> GetListWithNavigationPropertiesAsync(string? filterText = null, DateTime? dataAggiuntaMin = null, DateTime? dataAggiuntaMax = null, Guid? varianteProdottoId = null, Guid? listaDesideriId = null, string? sorting = null, int maxResultCount = int.MaxValue, int skipCount = 0, CancellationToken cancellationToken = default);
    Task<List<ElementoLista>> GetListAsync(string? filterText = null, DateTime? dataAggiuntaMin = null, DateTime? dataAggiuntaMax = null, string? sorting = null, int maxResultCount = int.MaxValue, int skipCount = 0, CancellationToken cancellationToken = default);
    Task<long> GetCountAsync(string? filterText = null, DateTime? dataAggiuntaMin = null, DateTime? dataAggiuntaMax = null, Guid? varianteProdottoId = null, Guid? listaDesideriId = null, CancellationToken cancellationToken = default);
}