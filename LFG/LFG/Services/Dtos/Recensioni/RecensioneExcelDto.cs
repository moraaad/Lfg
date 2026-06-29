using System;

namespace LFG.Recensioni;

public abstract class RecensioneExcelDtoBase
{
    public int Valutazione { get; set; }

    public string? Commento { get; set; }

    public DateTime DataRecensione { get; set; }
}