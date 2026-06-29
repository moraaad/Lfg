namespace LFG.Collezioni;

public static class CollezioneConsts
{
    private const string DefaultSorting = "{0}CreationTime desc";

    public static string GetDefaultSorting(bool withEntityName)
    {
        return string.Format(DefaultSorting, withEntityName ? "Collezione." : string.Empty);
    }

    public const int NomeMinLength = 2;
    public const int NomeMaxLength = 100;
    public const int StagioneMinLength = 2;
    public const int StagioneMaxLength = 100;
    public const int SezioneMinLength = 3;
    public const int SezioneMaxLength = 10;
}