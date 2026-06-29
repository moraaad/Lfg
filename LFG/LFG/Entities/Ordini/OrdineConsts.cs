namespace LFG.Ordini;

public static class OrdineConsts
{
    private const string DefaultSorting = "{0}CreationTime desc";

    public static string GetDefaultSorting(bool withEntityName)
    {
        return string.Format(DefaultSorting, withEntityName ? "Ordine." : string.Empty);
    }

    public const int StatoMaxLength = 50;
    public const int IndSpedizioneMaxLength = 100;
    public const int MetodoPagamentoMaxLength = 30;
}