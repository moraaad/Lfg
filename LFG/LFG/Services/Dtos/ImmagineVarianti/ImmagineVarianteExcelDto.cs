using System;

namespace LFG.ImmagineVarianti;

public abstract class ImmagineVarianteExcelDtoBase
{
    public Guid VarianteProdottoId { get; set; }

    public string Url { get; set; } = null!;
    public int Ordine { get; set; }
}