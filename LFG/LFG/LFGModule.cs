using LFG.ImmagineVarianti;
using LFG.ElementoListe;
using LFG.RigaOrdini;
using LFG.Recensioni;
using LFG.Pagamenti;
using LFG.Ordini;
using LFG.ListeDesideri;
using LFG.Indirizzi;
using LFG.VarianteProdotti;
using LFG.Prodotti;
using LFG.Clienti;
using LFG.Sconti;
using LFG.Collezioni;
using LFG.Categorie;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using Microsoft.AspNetCore.Authentication.Twitter;
using Microsoft.AspNetCore.Cors;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.OpenApi;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LFG.Data;
using LFG.Localization;
using LFG.Menus;
using LFG.Permissions;
using LFG.HealthChecks;
using OpenIddict.Validation.AspNetCore;
using System;
using Volo.Abp;
using Volo.Abp.Studio;
using Volo.Abp.Account;
using Volo.Abp.Account.Web;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Toolbars;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.Mapperly;
using Volo.Abp.Caching;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Identity.Web;
using Volo.Abp.Uow;
using Volo.Abp.Account.Admin.Web;
using Volo.Abp.Account.Public.Web;
using Volo.Abp.Account.Public.Web.ExternalProviders;
using Volo.Abp.Commercial.SuiteTemplates;
using Volo.Abp.Gdpr;
using Volo.Abp.Gdpr.Web;
using Volo.Abp.Gdpr.Web.Extensions;
using Volo.Abp.AuditLogging;
using Volo.Abp.AuditLogging.Web;
using Volo.Abp.LanguageManagement;
using Volo.Abp.TextTemplateManagement;
using Volo.Abp.TextTemplateManagement.Web;
using Volo.Abp.OpenIddict.Pro.Web;
using Microsoft.Extensions.AI;
using System.Linq;
using Volo.Abp.AI;
using Volo.AIManagement;
using Volo.AIManagement.Client;
using Volo.AIManagement.Factory;
using Volo.AIManagement.Web;
using Volo.Abp.Emailing;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Localization.Resources.AbpUi;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.PermissionManagement;
using Volo.Abp.PermissionManagement.HttpApi;
using Volo.Abp.PermissionManagement.Identity;
using Volo.Abp.PermissionManagement.Web;
using Volo.Abp.SettingManagement;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonX;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonX.Bundling;
using Volo.Abp.LeptonX.Shared;
using Volo.Abp.SettingManagement.Web;
using Volo.Abp.Swashbuckle;
using Volo.Abp.UI.Navigation;
using Volo.Abp.UI.Navigation.Urls;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;
using Volo.Abp.OpenIddict;
using Volo.Abp.PermissionManagement.OpenIddict;
using Volo.Abp.Security.Claims;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.TextTemplateManagement.EntityFrameworkCore;
using Volo.Abp.LanguageManagement.EntityFrameworkCore;
using Volo.AIManagement.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BlobStoring.Database.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.SqlServer;
using Volo.Abp.Studio.Client.AspNetCore;
using Microsoft.Extensions.Hosting;

namespace LFG;

[DependsOn(// ABP Framework packages
typeof(AbpAspNetCoreMvcModule), typeof(AbpAutofacModule), typeof(AbpMapperlyModule), typeof(AbpCachingModule), typeof(AbpSwashbuckleModule), typeof(AbpAspNetCoreSerilogModule), typeof(VoloAbpCommercialSuiteTemplatesModule), typeof(AbpStudioClientAspNetCoreModule),
// lepton-theme
typeof(AbpAspNetCoreMvcUiLeptonXThemeModule),
// Account module packages
typeof(AbpAccountPublicWebOpenIddictModule), typeof(AbpAccountPublicHttpApiModule), typeof(AbpAccountPublicApplicationModule), typeof(AbpAccountAdminWebModule), typeof(AbpAccountAdminHttpApiModule), typeof(AbpAccountAdminApplicationModule),
// OpenIddict module packages
typeof(AbpOpenIddictProWebModule), typeof(AbpOpenIddictProHttpApiModule), typeof(AbpOpenIddictProApplicationModule),
// Audit logging module packages
typeof(AbpAuditLoggingWebModule), typeof(AbpAuditLoggingHttpApiModule), typeof(AbpAuditLoggingApplicationModule),
// Language Management module packages
typeof(LanguageManagementWebModule), typeof(LanguageManagementHttpApiModule), typeof(LanguageManagementApplicationModule),
// GDPR module packages
typeof(AbpGdprApplicationModule), typeof(AbpGdprHttpApiModule), typeof(AbpGdprWebModule),
// Text Template Management module packages
typeof(TextTemplateManagementWebModule), typeof(TextTemplateManagementHttpApiModule), typeof(TextTemplateManagementApplicationModule),
// AI Management module packages
typeof(AIManagementWebModule), typeof(AIManagementHttpApiModule), typeof(AIManagementApplicationModule), typeof(AIManagementClientApplicationModule), typeof(AIManagementClientWebModule), typeof(AIManagementClientHttpApiModule),
// Identity module packages
typeof(AbpPermissionManagementDomainIdentityModule), typeof(AbpPermissionManagementDomainOpenIddictModule), typeof(AbpIdentityWebModule), typeof(AbpIdentityHttpApiModule), typeof(AbpIdentityApplicationModule),
// Permission Management module packages
typeof(AbpPermissionManagementWebModule), typeof(AbpPermissionManagementApplicationModule), typeof(AbpPermissionManagementHttpApiModule),
// Feature Management module packages
typeof(AbpFeatureManagementWebModule), typeof(AbpFeatureManagementHttpApiModule), typeof(AbpFeatureManagementApplicationModule),
// Setting Management module packages
typeof(AbpSettingManagementWebModule), typeof(AbpSettingManagementHttpApiModule), typeof(AbpSettingManagementApplicationModule),
// Entity Framework Core packages for the used modules
typeof(AbpAuditLoggingEntityFrameworkCoreModule), typeof(AbpFeatureManagementEntityFrameworkCoreModule), typeof(AbpGdprEntityFrameworkCoreModule), typeof(AbpIdentityProEntityFrameworkCoreModule), typeof(AbpOpenIddictProEntityFrameworkCoreModule), typeof(LanguageManagementEntityFrameworkCoreModule), typeof(TextTemplateManagementEntityFrameworkCoreModule), typeof(AIManagementEntityFrameworkCoreModule), typeof(AbpPermissionManagementEntityFrameworkCoreModule), typeof(AbpSettingManagementEntityFrameworkCoreModule), typeof(AbpBackgroundJobsEntityFrameworkCoreModule), typeof(BlobStoringDatabaseEntityFrameworkCoreModule), typeof(AbpEntityFrameworkCoreSqlServerModule))]
public class LFGModule : AbpModule
{
    /* Single point to enable/disable multi-tenancy */
    public const bool IsMultiTenant = false;

    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        var hostingEnvironment = context.Services.GetHostingEnvironment();
        var configuration = context.Services.GetConfiguration();
        context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options => {
            options.AddAssemblyResource(typeof(LFGResource));
        });
        PreConfigure<OpenIddictBuilder>(builder => {
            builder.AddValidation(options => {
                options.AddAudiences("LFG");
                options.UseLocalServer();
                options.UseAspNetCore();
            });
        });
        if (!hostingEnvironment.IsDevelopment())
        {
            PreConfigure<AbpOpenIddictAspNetCoreOptions>(options => {
                options.AddDevelopmentEncryptionAndSigningCertificate = false;
            });
            PreConfigure<OpenIddictServerBuilder>(serverBuilder => {
                serverBuilder.AddProductionEncryptionAndSigningCertificate("openiddict.pfx", configuration["AuthServer:CertificatePassPhrase"]!);
            });
        }

        LFGGlobalFeatureConfigurator.Configure();
        LFGModuleExtensionConfigurator.Configure();
        LFGEfCoreEntityExtensionMappings.Configure();
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var hostingEnvironment = context.Services.GetHostingEnvironment();
        var configuration = context.Services.GetConfiguration();
        if (hostingEnvironment.IsDevelopment())
        {
            context.Services.Replace(ServiceDescriptor.Singleton<IEmailSender, NullEmailSender>());
            context.Services.AddRazorPages().AddRazorRuntimeCompilation();
        }

        ConfigureStudio(hostingEnvironment);
        ConfigureAuthentication(context);
        ConfigureMultiTenancy();
        ConfigureUrls(configuration);
        ConfigureBundles(hostingEnvironment);
        ConfigureHealthChecks(context);
        ConfigureImpersonation(context, configuration);
        ConfigureCookieConsent(context);
        ConfigureExternalProviders(context.Services);
        ConfigureSwagger(context.Services);
        ConfigureAutoApiControllers();
        ConfigureVirtualFiles(hostingEnvironment);
        ConfigureLocalization();
        ConfigureNavigationServices();
        ConfigureEfCore(context);
        ConfigureTheme();
        Configure<RazorPagesOptions>(options => {
            options.Conventions.AuthorizePage("/Books/Index", LFGPermissions.Books.Default);
            options.Conventions.AuthorizePage("/Books/CreateModal", LFGPermissions.Books.Create);
            options.Conventions.AuthorizePage("/Books/EditModal", LFGPermissions.Books.Edit);
            options.Conventions.AuthorizePage("/HostDashboard", LFGPermissions.Dashboard.Host);
            options.Conventions.AuthorizePage("/Categorie/Index", LFGPermissions.Categorie.Default);
            options.Conventions.AuthorizePage("/Collezioni/Index", LFGPermissions.Collezioni.Default);
            options.Conventions.AuthorizePage("/Sconti/Index", LFGPermissions.Sconti.Default);
            options.Conventions.AuthorizePage("/Clienti/Index", LFGPermissions.Clienti.Default);
            options.Conventions.AuthorizePage("/Prodotti/Index", LFGPermissions.Prodotti.Default);
            options.Conventions.AuthorizePage("/Indirizzi/Index", LFGPermissions.Indirizzi.Default);
            options.Conventions.AuthorizePage("/ListeDesideri/Index", LFGPermissions.ListeDesideri.Default);
            options.Conventions.AuthorizePage("/Ordini/Index", LFGPermissions.Ordini.Default);
            options.Conventions.AuthorizePage("/Pagamenti/Index", LFGPermissions.Pagamenti.Default);
            options.Conventions.AuthorizePage("/Recensioni/Index", LFGPermissions.Recensioni.Default);
            options.Conventions.AuthorizePage("/RigaOrdini/Index", LFGPermissions.RigaOrdini.Default);
            options.Conventions.AuthorizePage("/ElementoListe/Index", LFGPermissions.ElementoListe.Default);
            options.Conventions.AuthorizePage("/ImmagineVarianti/Index", LFGPermissions.ImmagineVarianti.Default);
        });
    }

    private void ConfigureCookieConsent(ServiceConfigurationContext context)
    {
        context.Services.AddAbpCookieConsent(options => {
            options.IsEnabled = true;
            options.CookiePolicyUrl = "/CookiePolicy";
            options.PrivacyPolicyUrl = "/PrivacyPolicy";
        });
    }

    private void ConfigureHealthChecks(ServiceConfigurationContext context)
    {
        context.Services.AddLFGHealthChecks();
    }

    private void ConfigureTheme()
    {
        Configure<LeptonXThemeOptions>(options => {
            options.DefaultStyle = LeptonXStyleNames.System;
        });
        Configure<LeptonXThemeMvcOptions>(options => {
            options.ApplicationLayout = LeptonXMvcLayouts.SideMenu;
        });
    }

    private void ConfigureStudio(IHostEnvironment hostingEnvironment)
    {
        if (hostingEnvironment.IsProduction())
        {
            Configure<AbpStudioClientOptions>(options => {
                options.IsLinkEnabled = false;
            });
        }
    }

    private void ConfigureAuthentication(ServiceConfigurationContext context)
    {
        context.Services.ForwardIdentityAuthenticationForBearer(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);
        context.Services.Configure<AbpClaimsPrincipalFactoryOptions>(options => {
            options.IsDynamicClaimsEnabled = true;
        });
    }

    private void ConfigureMultiTenancy()
    {
        Configure<AbpMultiTenancyOptions>(options => {
            options.IsEnabled = IsMultiTenant;
        });
    }

    private void ConfigureUrls(IConfiguration configuration)
    {
        Configure<AppUrlOptions>(options => {
            options.Applications["MVC"].RootUrl = configuration["App:SelfUrl"];
        });
    }

    private void ConfigureBundles(IHostEnvironment hostingEnvironment)
    {
        Configure<AbpBundlingOptions>(options => {
            options.StyleBundles.Configure(LeptonXThemeBundles.Styles.Global, bundle => {
                bundle.AddFiles("/global-styles.css");
            });
            options.ScriptBundles.Configure(LeptonXThemeBundles.Scripts.Global, bundle => {
                bundle.AddFiles("/global-scripts.js");
                if (hostingEnvironment.IsDevelopment())
                {
                    bundle.AddFiles("/dev-login-helper.js");
                }
            });
        });
    }

    private void ConfigureLocalization()
    {
        Configure<AbpLocalizationOptions>(options => {
            options.Resources.Add<LFGResource>("en").AddBaseTypes(typeof(AbpValidationResource), typeof(AbpUiResource)).AddVirtualJson("/Localization/LFG");
            options.DefaultResourceType = typeof(LFGResource);
            options.Languages.Add(new LanguageInfo("en", "en", "English"));
            options.Languages.Add(new LanguageInfo("ar", "ar", "Arabic"));
            options.Languages.Add(new LanguageInfo("zh-Hans", "zh-Hans", "Chinese (Simplified)"));
            options.Languages.Add(new LanguageInfo("zh-Hant", "zh-Hant", "Chinese (Traditional)"));
            options.Languages.Add(new LanguageInfo("cs", "cs", "Czech"));
            options.Languages.Add(new LanguageInfo("en-GB", "en-GB", "English (UK)"));
            options.Languages.Add(new LanguageInfo("fi", "fi", "Finnish"));
            options.Languages.Add(new LanguageInfo("fr", "fr", "French"));
            options.Languages.Add(new LanguageInfo("de-DE", "de-DE", "German (Germany)"));
            options.Languages.Add(new LanguageInfo("hi", "hi", "Hindi "));
            options.Languages.Add(new LanguageInfo("hu", "hu", "Hungarian"));
            options.Languages.Add(new LanguageInfo("is", "is", "Icelandic"));
            options.Languages.Add(new LanguageInfo("it", "it", "Italian"));
            options.Languages.Add(new LanguageInfo("pt-BR", "pt-BR", "Portuguese (Brazil)"));
            options.Languages.Add(new LanguageInfo("ro-RO", "ro-RO", "Romanian (Romania)"));
            options.Languages.Add(new LanguageInfo("ru", "ru", "Russian"));
            options.Languages.Add(new LanguageInfo("sk", "sk", "Slovak"));
            options.Languages.Add(new LanguageInfo("es", "es", "Spanish"));
            options.Languages.Add(new LanguageInfo("sv", "sv", "Swedish"));
            options.Languages.Add(new LanguageInfo("tr", "tr", "Turkish"));
        });
        Configure<AbpExceptionLocalizationOptions>(options => {
            options.MapCodeNamespace("LFG", typeof(LFGResource));
        });
    }

    private void ConfigureVirtualFiles(IWebHostEnvironment hostingEnvironment)
    {
        Configure<AbpVirtualFileSystemOptions>(options => {
            options.FileSets.AddEmbedded<LFGModule>();
            if (hostingEnvironment.IsDevelopment())
            {
                /* Using physical files in development, so we don't need to recompile on changes */
                options.FileSets.ReplaceEmbeddedByPhysical<LFGModule>(hostingEnvironment.ContentRootPath);
            }
        });
    }

    private void ConfigureAutoApiControllers()
    {
        Configure<AbpAspNetCoreMvcOptions>(options => {
            options.ConventionalControllers.Create(typeof(LFGModule).Assembly);
        });
    }

    private void ConfigureSwagger(IServiceCollection services)
    {
        services.AddAbpSwaggerGen(options => {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "LFG API", Version = "v1" });
            options.DocInclusionPredicate((docName, description) => true);
            options.CustomSchemaIds(type => type.FullName);
        });
    }

    private void ConfigureImpersonation(ServiceConfigurationContext context, IConfiguration configuration)
    {
        context.Services.Configure<AbpIdentityWebOptions>(options => {
            options.EnableUserImpersonation = true;
        });
        context.Services.Configure<AbpAccountOptions>(options => {
            options.TenantAdminUserName = "admin";
            options.ImpersonationUserPermission = IdentityPermissions.Users.Impersonation;
        });
    }

    private void ConfigureExternalProviders(IServiceCollection services)
    {
        services.AddAuthentication().AddGoogle(GoogleDefaults.AuthenticationScheme, options => {
            options.ClaimActions.MapJsonKey(AbpClaimTypes.Picture, "picture");
        }).WithDynamicOptions<GoogleOptions, GoogleHandler>(GoogleDefaults.AuthenticationScheme, options => {
            options.WithProperty(x => x.ClientId);
            options.WithProperty(x => x.ClientSecret, isSecret: true);
        }).AddMicrosoftAccount(MicrosoftAccountDefaults.AuthenticationScheme, options => {
            //Personal Microsoft accounts as an example.
            options.AuthorizationEndpoint = "https://login.microsoftonline.com/consumers/oauth2/v2.0/authorize";
            options.TokenEndpoint = "https://login.microsoftonline.com/consumers/oauth2/v2.0/token";
            options.ClaimActions.MapCustomJson("picture", _ => "https://graph.microsoft.com/v1.0/me/photo/$value");
            options.SaveTokens = true;
        }).WithDynamicOptions<MicrosoftAccountOptions, MicrosoftAccountHandler>(MicrosoftAccountDefaults.AuthenticationScheme, options => {
            options.WithProperty(x => x.ClientId);
            options.WithProperty(x => x.ClientSecret, isSecret: true);
        }).AddTwitter(TwitterDefaults.AuthenticationScheme, options => {
            options.ClaimActions.MapJsonKey(AbpClaimTypes.Picture, "profile_image_url_https");
            options.RetrieveUserDetails = true;
        }).WithDynamicOptions<TwitterOptions, TwitterHandler>(TwitterDefaults.AuthenticationScheme, options => {
            options.WithProperty(x => x.ConsumerKey);
            options.WithProperty(x => x.ConsumerSecret, isSecret: true);
        });
    }

    private void ConfigureNavigationServices()
    {
        Configure<AbpNavigationOptions>(options => {
            options.MenuContributors.Add(new LFGMenuContributor());
        });
        Configure<AbpToolbarOptions>(options => {
            options.Contributors.Add(new LFGToolbarContributor());
        });
    }

    private void ConfigureEfCore(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<LFGDbContext>(options => {
            /* You can remove "includeAllEntities: true" to create
             * default repositories only for aggregate roots
             * Documentation: https://docs.abp.io/en/abp/latest/Entity-Framework-Core#add-default-repositories
             */
            options.AddDefaultRepositories(includeAllEntities: true); // includeAllEntities: true -> include anche le entità non aggregate root (es. VarianteProdotto).
            options.AddRepository<Categoria, Categorie.EfCoreCategoriaRepository>();
            options.AddRepository<Collezione, Collezioni.EfCoreCollezioneRepository>();
            options.AddRepository<Sconto, Sconti.EfCoreScontoRepository>();
            options.AddRepository<Cliente, Clienti.EfCoreClienteRepository>();
            options.AddRepository<Prodotto, Prodotti.EfCoreProdottoRepository>();
            options.AddRepository<VarianteProdotto, VarianteProdotti.EfCoreVarianteProdottoRepository>();
            options.AddRepository<Indirizzo, Indirizzi.EfCoreIndirizzoRepository>();
            options.AddRepository<ListaDesideri, ListeDesideri.EfCoreListaDesideriRepository>();
            options.AddRepository<Ordine, Ordini.EfCoreOrdineRepository>();
            options.AddRepository<Pagamento, Pagamenti.EfCorePagamentoRepository>();
            options.AddRepository<Recensione, Recensioni.EfCoreRecensioneRepository>();
            options.AddRepository<RigaOrdine, RigaOrdini.EfCoreRigaOrdineRepository>();
            options.AddRepository<ElementoLista, ElementoListe.EfCoreElementoListaRepository>();
            options.AddRepository<ImmagineVariante, ImmagineVarianti.EfCoreImmagineVarianteRepository>();
        });
        Configure<AbpDbContextOptions>(options => {
            options.Configure(configurationContext => {
                configurationContext.UseSqlServer();
            });
        });
    }

    // TODO: Remove this method after v10.0.2 is released. This is a temporary fix for issue in AI Management module.
    public override async System.Threading.Tasks.Task OnPostApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        if (Volo.Abp.Data.AbpDataMigrationEnvironmentExtensions.GetDataMigrationEnvironment(context.ServiceProvider) != null)
        {
            return;
        }
        // Manually update AI providers for the application. No need to do this in the newer versions than v10.0.1.
        var appAIProviderManager = context.ServiceProvider.GetRequiredService<Volo.AIManagement.ApplicationWorkspaceProviders.ApplicationAIProviderManager>();
        var appInfoAccessor = context.ServiceProvider.GetRequiredService<IApplicationInfoAccessor>();
        var factoryOptions = context.ServiceProvider.GetRequiredService<Microsoft.Extensions.Options.IOptions<Volo.AIManagement.Factory.ChatClientFactoryOptions>>();
        await appAIProviderManager.UpdateProvidersAsync(appInfoAccessor.ApplicationName!, factoryOptions.Value.Factories.Keys.ToArray());
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseAbpRequestLocalization();
        if (!env.IsDevelopment())
        {
            app.UseErrorPage();
        }

        app.UseAbpCookieConsent();
        app.UseCorrelationId();
        app.UseRouting();
        app.MapAbpStaticAssets();
        app.UseAbpStudioLink();
        app.UseAbpSecurityHeaders();
        app.UseAuthentication();
        app.UseAbpOpenIddictValidation();
        if (IsMultiTenant)
        {
            app.UseMultiTenancy();
        }

        app.UseUnitOfWork();
        app.UseDynamicClaims();
        app.UseAuthorization();
        app.UseSwagger();
        app.UseAbpSwaggerUI(options => {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "LFG API");
        });
        app.UseAuditing();
        app.UseAbpSerilogEnrichers();
        app.UseConfiguredEndpoints();
    }
}