using System;

namespace LFG.Pagamenti;

public abstract class PagamentoExcelDtoBase
{
    public string? Metodo { get; set; }

    public string? Stato { get; set; }

    public decimal Importo { get; set; }

    public DateTime DataPagamento { get; set; }

    public string IdTransazione { get; set; } = null!;
}