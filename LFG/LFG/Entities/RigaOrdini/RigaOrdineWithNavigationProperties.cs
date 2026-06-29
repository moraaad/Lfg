using LFG.Ordini;
using LFG.VarianteProdotti;
using System;
using System.Collections.Generic;
using LFG.RigaOrdini;

namespace LFG.RigaOrdini;

public abstract class RigaOrdineWithNavigationPropertiesBase
{
    public RigaOrdine RigaOrdine { get; set; } = null!;
    public Ordine? Ordine { get; set; }

    public VarianteProdotto? VarianteProdotto { get; set; }
}