using System;

namespace LFG.Categorie;

public abstract class CategoriaExcelDtoBase
{
    public string Nome { get; set; } = null!;
    public string? Descrizione { get; set; }

    public string Sezione { get; set; } = null!;
}