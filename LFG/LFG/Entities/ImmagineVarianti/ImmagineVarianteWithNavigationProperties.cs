using LFG.VarianteProdotti;
using System;
using System.Collections.Generic;
using LFG.ImmagineVarianti;

namespace LFG.ImmagineVarianti;

public abstract class ImmagineVarianteWithNavigationPropertiesBase
{
    public ImmagineVariante ImmagineVariante { get; set; } = null!;
    public VarianteProdotto? VarianteProdotto { get; set; }
}