using LFG.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace LFG.Permissions;

public class LFGPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(LFGPermissions.GroupName);
        myGroup.AddPermission(LFGPermissions.Dashboard.Host, L("Permission:Dashboard"), MultiTenancySides.Host);
        var booksPermission = myGroup.AddPermission(LFGPermissions.Books.Default, L("Permission:Books"));
        booksPermission.AddChild(LFGPermissions.Books.Create, L("Permission:Books.Create"));
        booksPermission.AddChild(LFGPermissions.Books.Edit, L("Permission:Books.Edit"));
        booksPermission.AddChild(LFGPermissions.Books.Delete, L("Permission:Books.Delete"));
        //Define your own permissions here. Example:
        //myGroup.AddPermission(LFGPermissions.MyPermission1, L("Permission:MyPermission1"));
        var categoriaPermission = myGroup.AddPermission(LFGPermissions.Categorie.Default, L("Permission:Categorie"));
        categoriaPermission.AddChild(LFGPermissions.Categorie.Create, L("Permission:Create"));
        categoriaPermission.AddChild(LFGPermissions.Categorie.Edit, L("Permission:Edit"));
        categoriaPermission.AddChild(LFGPermissions.Categorie.Delete, L("Permission:Delete"));
        var collezionePermission = myGroup.AddPermission(LFGPermissions.Collezioni.Default, L("Permission:Collezioni"));
        collezionePermission.AddChild(LFGPermissions.Collezioni.Create, L("Permission:Create"));
        collezionePermission.AddChild(LFGPermissions.Collezioni.Edit, L("Permission:Edit"));
        collezionePermission.AddChild(LFGPermissions.Collezioni.Delete, L("Permission:Delete"));
        var scontoPermission = myGroup.AddPermission(LFGPermissions.Sconti.Default, L("Permission:Sconti"));
        scontoPermission.AddChild(LFGPermissions.Sconti.Create, L("Permission:Create"));
        scontoPermission.AddChild(LFGPermissions.Sconti.Edit, L("Permission:Edit"));
        scontoPermission.AddChild(LFGPermissions.Sconti.Delete, L("Permission:Delete"));
        var clientePermission = myGroup.AddPermission(LFGPermissions.Clienti.Default, L("Permission:Clienti"));
        clientePermission.AddChild(LFGPermissions.Clienti.Create, L("Permission:Create"));
        clientePermission.AddChild(LFGPermissions.Clienti.Edit, L("Permission:Edit"));
        clientePermission.AddChild(LFGPermissions.Clienti.Delete, L("Permission:Delete"));
        var prodottoPermission = myGroup.AddPermission(LFGPermissions.Prodotti.Default, L("Permission:Prodotti"));
        prodottoPermission.AddChild(LFGPermissions.Prodotti.Create, L("Permission:Create"));
        prodottoPermission.AddChild(LFGPermissions.Prodotti.Edit, L("Permission:Edit"));
        prodottoPermission.AddChild(LFGPermissions.Prodotti.Delete, L("Permission:Delete"));
        var varianteProdottoPermission = myGroup.AddPermission(LFGPermissions.VarianteProdotti.Default, L("Permission:VarianteProdotti"));
        varianteProdottoPermission.AddChild(LFGPermissions.VarianteProdotti.Create, L("Permission:Create"));
        varianteProdottoPermission.AddChild(LFGPermissions.VarianteProdotti.Edit, L("Permission:Edit"));
        varianteProdottoPermission.AddChild(LFGPermissions.VarianteProdotti.Delete, L("Permission:Delete"));
        var indirizzoPermission = myGroup.AddPermission(LFGPermissions.Indirizzi.Default, L("Permission:Indirizzi"));
        indirizzoPermission.AddChild(LFGPermissions.Indirizzi.Create, L("Permission:Create"));
        indirizzoPermission.AddChild(LFGPermissions.Indirizzi.Edit, L("Permission:Edit"));
        indirizzoPermission.AddChild(LFGPermissions.Indirizzi.Delete, L("Permission:Delete"));
        var listaDesideriPermission = myGroup.AddPermission(LFGPermissions.ListeDesideri.Default, L("Permission:ListeDesideri"));
        listaDesideriPermission.AddChild(LFGPermissions.ListeDesideri.Create, L("Permission:Create"));
        listaDesideriPermission.AddChild(LFGPermissions.ListeDesideri.Edit, L("Permission:Edit"));
        listaDesideriPermission.AddChild(LFGPermissions.ListeDesideri.Delete, L("Permission:Delete"));
        var ordinePermission = myGroup.AddPermission(LFGPermissions.Ordini.Default, L("Permission:Ordini"));
        ordinePermission.AddChild(LFGPermissions.Ordini.Create, L("Permission:Create"));
        ordinePermission.AddChild(LFGPermissions.Ordini.Edit, L("Permission:Edit"));
        ordinePermission.AddChild(LFGPermissions.Ordini.Delete, L("Permission:Delete"));
        var pagamentoPermission = myGroup.AddPermission(LFGPermissions.Pagamenti.Default, L("Permission:Pagamenti"));
        pagamentoPermission.AddChild(LFGPermissions.Pagamenti.Create, L("Permission:Create"));
        pagamentoPermission.AddChild(LFGPermissions.Pagamenti.Edit, L("Permission:Edit"));
        pagamentoPermission.AddChild(LFGPermissions.Pagamenti.Delete, L("Permission:Delete"));
        var recensionePermission = myGroup.AddPermission(LFGPermissions.Recensioni.Default, L("Permission:Recensioni"));
        recensionePermission.AddChild(LFGPermissions.Recensioni.Create, L("Permission:Create"));
        recensionePermission.AddChild(LFGPermissions.Recensioni.Edit, L("Permission:Edit"));
        recensionePermission.AddChild(LFGPermissions.Recensioni.Delete, L("Permission:Delete"));
        var rigaOrdinePermission = myGroup.AddPermission(LFGPermissions.RigaOrdini.Default, L("Permission:RigaOrdini"));
        rigaOrdinePermission.AddChild(LFGPermissions.RigaOrdini.Create, L("Permission:Create"));
        rigaOrdinePermission.AddChild(LFGPermissions.RigaOrdini.Edit, L("Permission:Edit"));
        rigaOrdinePermission.AddChild(LFGPermissions.RigaOrdini.Delete, L("Permission:Delete"));
        var elementoListaPermission = myGroup.AddPermission(LFGPermissions.ElementoListe.Default, L("Permission:ElementoListe"));
        elementoListaPermission.AddChild(LFGPermissions.ElementoListe.Create, L("Permission:Create"));
        elementoListaPermission.AddChild(LFGPermissions.ElementoListe.Edit, L("Permission:Edit"));
        elementoListaPermission.AddChild(LFGPermissions.ElementoListe.Delete, L("Permission:Delete"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<LFGResource>(name);
    }
}