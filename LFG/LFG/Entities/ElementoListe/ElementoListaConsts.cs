namespace LFG.ElementoListe;

public static class ElementoListaConsts
{
    private const string DefaultSorting = "{0}CreationTime desc";

    public static string GetDefaultSorting(bool withEntityName)
    {
        return string.Format(DefaultSorting, withEntityName ? "ElementoLista." : string.Empty);
    }
}