namespace LFG.Sconti;

public static class ScontoConsts
{
    private const string DefaultSorting = "{0}CreationTime desc";

    public static string GetDefaultSorting(bool withEntityName)
    {
        return string.Format(DefaultSorting, withEntityName ? "Sconto." : string.Empty);
    }

    public const int CodiceMinLength = 2;
    public const int CodiceMaxLength = 50;
    public const int SezioneMinLength = 2;
    public const int SezioneMaxLength = 3;
}