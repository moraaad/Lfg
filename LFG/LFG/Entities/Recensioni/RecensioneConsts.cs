namespace LFG.Recensioni;

public static class RecensioneConsts
{
    private const string DefaultSorting = "{0}CreationTime desc";

    public static string GetDefaultSorting(bool withEntityName)
    {
        return string.Format(DefaultSorting, withEntityName ? "Recensione." : string.Empty);
    }

    public const int ValutazioneMinLength = 1;
    public const int ValutazioneMaxLength = 5;
    public const int CommentoMaxLength = 1000;
}