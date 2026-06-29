using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace LFG.Data;

public class LFGDbContextFactory : IDesignTimeDbContextFactory<LFGDbContext>
{
    public LFGDbContext CreateDbContext(string[] args)
    {
        LFGGlobalFeatureConfigurator.Configure();
        LFGModuleExtensionConfigurator.Configure();

        LFGEfCoreEntityExtensionMappings.Configure();
        var configuration = BuildConfiguration();

        var builder = new DbContextOptionsBuilder<LFGDbContext>()
            .UseSqlServer(configuration.GetConnectionString("Default"));

        return new LFGDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .AddEnvironmentVariables();

        return builder.Build();
    }
}