using System;

namespace LFG.Prodotti;

public abstract class ProdottoExcelDtoBase
{
    public string Nome { get; set; } = null!;
    public string? Descrizione { get; set; }

    public string Prezzo { get; set; } = null!;
    public string? CodiceSku { get; set; }

    public string Sezione { get; set; } = null!;
}