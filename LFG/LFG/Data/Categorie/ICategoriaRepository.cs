using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace LFG.Categorie;

public partial interface ICategoriaRepository : IRepository<Categoria, Guid>
{
    Task DeleteAllAsync(string? filterText = null, string? nome = null, string? descrizione = null, string? sezione = null, CancellationToken cancellationToken = default);
    Task<List<Categoria>> GetListAsync(string? filterText = null, string? nome = null, string? descrizione = null, string? sezione = null, string? sorting = null, int maxResultCount = int.MaxValue, int skipCount = 0, CancellationToken cancellationToken = default);
    Task<long> GetCountAsync(string? filterText = null, string? nome = null, string? descrizione = null, string? sezione = null, CancellationToken cancellationToken = default);
}