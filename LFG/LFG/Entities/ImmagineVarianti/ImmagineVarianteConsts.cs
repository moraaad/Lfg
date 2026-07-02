namespace LFG.ImmagineVarianti;

public static class ImmagineVarianteConsts
{
    private const string DefaultSorting = "{0}CreationTime desc";

    public static string GetDefaultSorting(bool withEntityName)
    {
        return string.Format(DefaultSorting, withEntityName ? "ImmagineVariante." : string.Empty);
    }

    public const int UrlMaxLength = 500;
}