namespace LFG.Categorie;

public static class CategoriaConsts
{
    private const string DefaultSorting = "{0}CreationTime desc";

    public static string GetDefaultSorting(bool withEntityName)
    {
        return string.Format(DefaultSorting, withEntityName ? "Categoria." : string.Empty);
    }

    public const int NomeMinLength = 2;
    public const int NomeMaxLength = 100;
    public const int DescrizioneMaxLength = 500;
    public const int SezioneMinLength = 3;
    public const int SezioneMaxLength = 10;
}