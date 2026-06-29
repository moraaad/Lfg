using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Sqlite;
using LFG.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using LFG.Menus;
using Volo.Abp;
using Volo.Abp.AspNetCore.TestBase;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Data;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Modularity;
using Volo.Abp.OpenIddict;
using Volo.Abp.PermissionManagement;
using Volo.Abp.TextTemplateManagement;
using Volo.Abp.LanguageManagement.External;
using Volo.Abp.Threading;
using Volo.Abp.UI.Navigation;
using Volo.Abp.Uow;

namespace LFG;

[DependsOn(
    typeof(AbpTestBaseModule),
    typeof(AbpAspNetCoreTestBaseModule),
    typeof(AbpEntityFrameworkCoreSqliteModule),
    typeof(LFGModule)
)]
public class LFGTestModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        var builder = new ConfigurationBuilder();
        builder.AddJsonFile("appsettings.json", false);
        builder.AddJsonFile("appsettings.secrets.json", true);
        context.Services.ReplaceConfiguration(builder.Build());

        context.Services.PreConfigure<IMvcBuilder>(builder =>
        {
            builder.PartManager.ApplicationParts.Add(new CompiledRazorAssemblyPart(typeof(LFGModule).Assembly));
        });

        context.Services.GetPreConfigureActions<OpenIddictServerBuilder>().Clear();
        PreConfigure<AbpOpenIddictAspNetCoreOptions>(options =>
        {
            options.AddDevelopmentEncryptionAndSigningCertificate = true;
        });
        PreConfigure<AbpSqliteOptions>(x => x.BusyTimeout = null);
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new LFGMenuContributor());
        });

        Configure<AbpBackgroundJobOptions>(options =>
        {
            options.IsJobExecutionEnabled = false;
        });

        context.Services.AddAlwaysAllowAuthorization();
        context.Services.AddAlwaysDisableUnitOfWorkTransaction();

        Configure<FeatureManagementOptions>(options =>
        {
            options.SaveStaticFeaturesToDatabase = false;
            options.IsDynamicFeatureStoreEnabled = false;
        });
        Configure<PermissionManagementOptions>(options =>
        {
            options.SaveStaticPermissionsToDatabase = false;
            options.IsDynamicPermissionStoreEnabled = false;
        });

        Configure<TextTemplateManagementOptions>(options =>
        {
            options.SaveStaticTemplatesToDatabase = false;
            options.IsDynamicTemplateStoreEnabled = false;
        });
        Configure<AbpExternalLocalizationOptions>(options =>
        {
            options.SaveToExternalStore = false;
        });

        ConfigureInMemorySqlite(context.Services);
    }
    
    private SqliteConnection? _sqliteConnection;

    private void ConfigureInMemorySqlite(IServiceCollection services)
    {
        _sqliteConnection = CreateDatabaseAndGetConnection();

        services.Configure<AbpDbContextOptions>(options =>
        {
            options.Configure(context =>
            {
                context.DbContextOptions.UseSqlite(_sqliteConnection);
            });
        });
    }

    private static SqliteConnection CreateDatabaseAndGetConnection()
    {
        var connection = new SqliteConnection("Data Source=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<LFGDbContext>()
            .UseSqlite(connection)
            .Options;

        using (var context = new LFGDbContext(options))
        {
            context.GetService<IRelationalDatabaseCreator>().CreateTables();
        }

        return connection;
    }
    
    public override void OnApplicationShutdown(ApplicationShutdownContext context)
    {
        _sqliteConnection?.Dispose();
    }
    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        SeedTestData(context);
    }

    private static void SeedTestData(ApplicationInitializationContext context)
    {
        AsyncHelper.RunSync(async () =>
        {
            using (var scope = context.ServiceProvider.CreateScope())
            {
                await scope.ServiceProvider
                    .GetRequiredService<IDataSeeder>()
                    .SeedAsync();
            }
        });
    }
}
