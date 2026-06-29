using System;

namespace LFG.Indirizzi;

public abstract class IndirizzoExcelDtoBase
{
    public string? Paese { get; set; }

    public string? Citta { get; set; }

    public string? Provincia { get; set; }

    public string Via { get; set; } = null!;
    public string Cap { get; set; } = null!;
}