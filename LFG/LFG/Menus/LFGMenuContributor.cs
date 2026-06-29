using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using LFG.Permissions;
using LFG.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Identity.Web.Navigation;
using Volo.Abp.SettingManagement.Web.Navigation;
using Volo.Abp.UI.Navigation;
using Volo.Abp.AuditLogging.Web.Navigation;
using Volo.Abp.LanguageManagement.Navigation;
using Volo.Abp.OpenIddict.Pro.Web.Menus;
using Volo.Abp.TextTemplateManagement.Web.Navigation;
using Volo.AIManagement.Web.Menus;

namespace LFG.Menus;

public class LFGMenuContributor : IMenuContributor
{
    public async Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name == StandardMenus.Main)
        {
            await ConfigureMainMenuAsync(context);
        }
    }

    private static Task ConfigureMainMenuAsync(MenuConfigurationContext context)
    {
        var l = context.GetLocalizer<LFGResource>();
        context.Menu.Items.Insert(0, new ApplicationMenuItem(LFGMenus.Home, l["Menu:Home"], "~/", icon: "fas fa-home", order: 0));
        //HostDashboard
        context.Menu.AddItem(new ApplicationMenuItem(LFGMenus.HostDashboard, l["Menu:Dashboard"], "~/HostDashboard", icon: "fa fa-chart-line", order: 2).RequirePermissions(LFGPermissions.Dashboard.Host));
        //Administration
        var administration = context.Menu.GetAdministration();
        administration.Order = 5;
        //Administration->OpenIddict
        administration.SetSubItemOrder(OpenIddictProMenus.GroupName, 3);
        //Administration->Language Management
        administration.SetSubItemOrder(LanguageManagementMenuNames.GroupName, 4);
        //Administration->AI Management
        administration.SetSubItemOrder(AIManagementMenus.GroupName, 5);
        //Administration->Text Template Management
        administration.SetSubItemOrder(TextTemplateManagementMainMenuNames.GroupName, 6);
        //Administration->Audit Logs
        administration.SetSubItemOrder(AbpAuditLoggingMainMenuNames.GroupName, 7);
        //Administration->Identity
        administration.SetSubItemOrder(IdentityMenuNames.GroupName, 2);
        //Administration->Settings
        administration.SetSubItemOrder(SettingManagementMenuNames.GroupName, 8);
        context.Menu.AddItem(new ApplicationMenuItem("BooksStore", l["Menu:LFG"], icon: "fa fa-book").AddItem(new ApplicationMenuItem("BooksStore.Books", l["Menu:Books"], url: "/Books").RequirePermissions(LFGPermissions.Books.Default)));
        context.Menu.AddItem(new ApplicationMenuItem(LFGMenus.Categorie, l["Menu:Categorie"], url: "/Categorie", icon: "fa fa-file-alt", requiredPermissionName: LFGPermissions.Categorie.Default));
        context.Menu.AddItem(new ApplicationMenuItem(LFGMenus.Collezioni, l["Menu:Collezioni"], url: "/Collezioni", icon: "fa fa-file-alt", requiredPermissionName: LFGPermissions.Collezioni.Default));
        context.Menu.AddItem(new ApplicationMenuItem(LFGMenus.Sconti, l["Menu:Sconti"], url: "/Sconti", icon: "fa fa-file-alt", requiredPermissionName: LFGPermissions.Sconti.Default));
        context.Menu.AddItem(new ApplicationMenuItem(LFGMenus.Clienti, l["Menu:Clienti"], url: "/Clienti", icon: "fa fa-file-alt", requiredPermissionName: LFGPermissions.Clienti.Default));
        context.Menu.AddItem(new ApplicationMenuItem(LFGMenus.Prodotti, l["Menu:Prodotti"], url: "/Prodotti", icon: "fa fa-file-alt", requiredPermissionName: LFGPermissions.Prodotti.Default));
        context.Menu.AddItem(new ApplicationMenuItem(LFGMenus.Indirizzi, l["Menu:Indirizzi"], url: "/Indirizzi", icon: "fa fa-file-alt", requiredPermissionName: LFGPermissions.Indirizzi.Default));
        context.Menu.AddItem(new ApplicationMenuItem(LFGMenus.ListeDesideri, l["Menu:ListeDesideri"], url: "/ListeDesideri", icon: "fa fa-file-alt", requiredPermissionName: LFGPermissions.ListeDesideri.Default));
        context.Menu.AddItem(new ApplicationMenuItem(LFGMenus.Ordini, l["Menu:Ordini"], url: "/Ordini", icon: "fa fa-file-alt", requiredPermissionName: LFGPermissions.Ordini.Default));
        context.Menu.AddItem(new ApplicationMenuItem(LFGMenus.Pagamenti, l["Menu:Pagamenti"], url: "/Pagamenti", icon: "fa fa-file-alt", requiredPermissionName: LFGPermissions.Pagamenti.Default));
        context.Menu.AddItem(new ApplicationMenuItem(LFGMenus.Recensioni, l["Menu:Recensioni"], url: "/Recensioni", icon: "fa fa-file-alt", requiredPermissionName: LFGPermissions.Recensioni.Default));
        context.Menu.AddItem(new ApplicationMenuItem(LFGMenus.RigaOrdini, l["Menu:RigaOrdini"], url: "/RigaOrdini", icon: "fa fa-file-alt", requiredPermissionName: LFGPermissions.RigaOrdini.Default));
        context.Menu.AddItem(new ApplicationMenuItem(LFGMenus.ElementoListe, l["Menu:ElementoListe"], url: "/ElementoListe", icon: "fa fa-file-alt", requiredPermissionName: LFGPermissions.ElementoListe.Default));
        return Task.CompletedTask;
    }
}