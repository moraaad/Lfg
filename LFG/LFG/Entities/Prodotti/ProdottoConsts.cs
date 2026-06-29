namespace LFG.Prodotti;

public static class ProdottoConsts
{
    private const string DefaultSorting = "{0}CreationTime desc";

    public static string GetDefaultSorting(bool withEntityName)
    {
        return string.Format(DefaultSorting, withEntityName ? "Prodotto." : string.Empty);
    }

    public const int NomeMinLength = 2;
    public const int NomeMaxLength = 150;
    public const int DescrizioneMaxLength = 1000;
    public const int CodiceSkuMaxLength = 50;
    public const int SezioneMinLength = 2;
    public const int SezioneMaxLength = 3;
}