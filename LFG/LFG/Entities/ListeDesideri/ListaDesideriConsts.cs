namespace LFG.ListeDesideri;

public static class ListaDesideriConsts
{
    private const string DefaultSorting = "{0}CreationTime desc";

    public static string GetDefaultSorting(bool withEntityName)
    {
        return string.Format(DefaultSorting, withEntityName ? "ListaDesideri." : string.Empty);
    }

    public const int NomeListaMinLength = 2;
    public const int NomeListaMaxLength = 50;
}