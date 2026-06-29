namespace LFG.Pagamenti;

public static class PagamentoConsts
{
    private const string DefaultSorting = "{0}CreationTime desc";

    public static string GetDefaultSorting(bool withEntityName)
    {
        return string.Format(DefaultSorting, withEntityName ? "Pagamento." : string.Empty);
    }

    public const int IdTransazioneMaxLength = 100;
}