using LFG.Clienti;
using LFG.Sconti;
using LFG.Indirizzi;
using System;
using System.Collections.Generic;
using LFG.Ordini;

namespace LFG.Ordini;

public abstract class OrdineWithNavigationPropertiesBase
{
    public Ordine Ordine { get; set; } = null!;
    public Cliente? Cliente { get; set; }

    public Sconto? Sconto { get; set; }

    public Indirizzo? Indirizzo { get; set; }
}