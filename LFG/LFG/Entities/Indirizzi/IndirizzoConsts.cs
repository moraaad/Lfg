namespace LFG.Indirizzi;

public static class IndirizzoConsts
{
    private const string DefaultSorting = "{0}CreationTime desc";

    public static string GetDefaultSorting(bool withEntityName)
    {
        return string.Format(DefaultSorting, withEntityName ? "Indirizzo." : string.Empty);
    }

    public const int ViaMaxLength = 100;
    public const int CapMinLength = 4;
    public const int CapMaxLength = 5;
}