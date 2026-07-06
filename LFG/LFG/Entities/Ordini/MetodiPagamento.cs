namespace LFG.Ordini;

public static class MetodiPagamento
{
    public const string CartaDiCredito = "Carta di credito";
    public const string Bancomat = "Bancomat";
    public const string Buoni = "Buoni";
    public const string GiftCard = "Gift card";

    public static readonly string[] Tutti =
    {
        CartaDiCredito,
        Bancomat,
        Buoni,
        GiftCard
    };
}
