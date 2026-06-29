using LFG.Clienti;
using LFG.Sconti;
using System;
using System.Collections.Generic;
using LFG.Ordini;

namespace LFG.Ordini;

public abstract class OrdineWithNavigationPropertiesBase
{
    public Ordine Ordine { get; set; } = null!;
    public Cliente? Cliente { get; set; }

    public Sconto? Sconto { get; set; }
}