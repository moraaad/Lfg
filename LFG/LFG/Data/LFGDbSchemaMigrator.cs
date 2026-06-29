using Volo.Abp.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace LFG.Data;

public class LFGDbSchemaMigrator : ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public LFGDbSchemaMigrator(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        
        /* We intentionally resolving the LFGDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<LFGDbContext>()
            .Database
            .MigrateAsync();

    }
}
