namespace LFG.RigaOrdini;

public static class RigaOrdineConsts
{
    private const string DefaultSorting = "{0}CreationTime desc";

    public static string GetDefaultSorting(bool withEntityName)
    {
        return string.Format(DefaultSorting, withEntityName ? "RigaOrdine." : string.Empty);
    }

    public const int QuantitaMinLength = 1;
    public const int QuantitaMaxLength = 300;
    public const decimal PrezzoUnitarioMinLength = 1;
    public const decimal PrezzoUnitarioMaxLength = 1000000;
}