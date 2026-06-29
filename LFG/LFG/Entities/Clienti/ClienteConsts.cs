namespace LFG.Clienti;

public static class ClienteConsts
{
    private const string DefaultSorting = "{0}CreationTime desc";

    public static string GetDefaultSorting(bool withEntityName)
    {
        return string.Format(DefaultSorting, withEntityName ? "Cliente." : string.Empty);
    }

    public const int NomeMinLength = 2;
    public const int NomeMaxLength = 100;
    public const int CognomeMinLength = 2;
    public const int CognomeMaxLength = 100;
    public const int EmailMinLength = 2;
    public const int EmailMaxLength = 150;
    public const int TelefonoMinLength = 9;
    public const int TelefonoMaxLength = 10;
    public const int SezioneMinLength = 2;
    public const int SezioneMaxLength = 3;
}