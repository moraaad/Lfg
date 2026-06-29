namespace LFG.VarianteProdotti;

public static class VarianteProdottoConsts
{
    private const string DefaultSorting = "{0}CreationTime desc";

    public static string GetDefaultSorting(bool withEntityName)
    {
        return string.Format(DefaultSorting, withEntityName ? "VarianteProdotto." : string.Empty);
    }

    public const int UrlImmagineMaxLength = 300;
}