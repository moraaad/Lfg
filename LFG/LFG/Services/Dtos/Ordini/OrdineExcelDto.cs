using System;

namespace LFG.Ordini;

public abstract class OrdineExcelDtoBase
{
    public DateTime DataOrdine { get; set; }

    public string? Stato { get; set; }

    public decimal ImportoTotale { get; set; }

    public string? IndSpedizione { get; set; }

    public string? MetodoPagamento { get; set; }
}