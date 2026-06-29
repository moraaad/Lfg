namespace LFG.Permissions;

public static class LFGPermissions
{
    public const string GroupName = "LFG";

    public static class Dashboard
    {
        public const string DashboardGroup = GroupName + ".Dashboard";
        public const string Host = DashboardGroup + ".Host";
    }

    public static class Books
    {
        public const string Default = GroupName + ".Books";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }

    //Add your own permission names. Example:
    //public const string MyPermission1 = GroupName + ".MyPermission1";
    public static class Categorie
    {
        public const string Default = GroupName + ".Categorie";
        public const string Edit = Default + ".Edit";
        public const string Create = Default + ".Create";
        public const string Delete = Default + ".Delete";
    }

    public static class Collezioni
    {
        public const string Default = GroupName + ".Collezioni";
        public const string Edit = Default + ".Edit";
        public const string Create = Default + ".Create";
        public const string Delete = Default + ".Delete";
    }

    public static class Sconti
    {
        public const string Default = GroupName + ".Sconti";
        public const string Edit = Default + ".Edit";
        public const string Create = Default + ".Create";
        public const string Delete = Default + ".Delete";
    }

    public static class Clienti
    {
        public const string Default = GroupName + ".Clienti";
        public const string Edit = Default + ".Edit";
        public const string Create = Default + ".Create";
        public const string Delete = Default + ".Delete";
    }

    public static class Prodotti
    {
        public const string Default = GroupName + ".Prodotti";
        public const string Edit = Default + ".Edit";
        public const string Create = Default + ".Create";
        public const string Delete = Default + ".Delete";
    }

    public static class VarianteProdotti
    {
        public const string Default = GroupName + ".VarianteProdotti";
        public const string Edit = Default + ".Edit";
        public const string Create = Default + ".Create";
        public const string Delete = Default + ".Delete";
    }

    public static class Indirizzi
    {
        public const string Default = GroupName + ".Indirizzi";
        public const string Edit = Default + ".Edit";
        public const string Create = Default + ".Create";
        public const string Delete = Default + ".Delete";
    }

    public static class ListeDesideri
    {
        public const string Default = GroupName + ".ListeDesideri";
        public const string Edit = Default + ".Edit";
        public const string Create = Default + ".Create";
        public const string Delete = Default + ".Delete";
    }

    public static class Ordini
    {
        public const string Default = GroupName + ".Ordini";
        public const string Edit = Default + ".Edit";
        public const string Create = Default + ".Create";
        public const string Delete = Default + ".Delete";
    }

    public static class Pagamenti
    {
        public const string Default = GroupName + ".Pagamenti";
        public const string Edit = Default + ".Edit";
        public const string Create = Default + ".Create";
        public const string Delete = Default + ".Delete";
    }

    public static class Recensioni
    {
        public const string Default = GroupName + ".Recensioni";
        public const string Edit = Default + ".Edit";
        public const string Create = Default + ".Create";
        public const string Delete = Default + ".Delete";
    }

    public static class RigaOrdini
    {
        public const string Default = GroupName + ".RigaOrdini";
        public const string Edit = Default + ".Edit";
        public const string Create = Default + ".Create";
        public const string Delete = Default + ".Delete";
    }

    public static class ElementoListe
    {
        public const string Default = GroupName + ".ElementoListe";
        public const string Edit = Default + ".Edit";
        public const string Create = Default + ".Create";
        public const string Delete = Default + ".Delete";
    }
}