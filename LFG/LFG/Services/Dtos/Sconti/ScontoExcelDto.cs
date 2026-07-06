using System;

namespace LFG.Sconti;

public abstract class ScontoExcelDtoBase
{
    public string Codice { get; set; } = null!;
    public string? Tipo { get; set; }

    public decimal Valore { get; set; }

    public int? LimiteUtilizzi { get; set; }

    public DateTime ValidoDal { get; set; }

    public DateTime ValidoAl { get; set; }

    public string Sezione { get; set; } = null!;
}