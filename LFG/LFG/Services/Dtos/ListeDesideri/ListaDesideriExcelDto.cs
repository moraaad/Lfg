using System;

namespace LFG.ListeDesideri;

public abstract class ListaDesideriExcelDtoBase
{
    public DateTime DataCreazione { get; set; }

    public string NomeLista { get; set; } = null!;
}