using LFG.Ordini;
using LFG.VarianteProdotti;
using System;
using Volo.Abp.Application.Dtos;
using System.Collections.Generic;

namespace LFG.RigaOrdini;

public abstract class RigaOrdineWithNavigationPropertiesDtoBase
{
    public RigaOrdineDto RigaOrdine { get; set; } = null!;
    public OrdineDto? Ordine { get; set; }

    public VarianteProdottoDto? VarianteProdotto { get; set; }
}