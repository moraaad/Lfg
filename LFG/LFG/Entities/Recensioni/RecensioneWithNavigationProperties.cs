using LFG.Clienti;
using LFG.Prodotti;
using System;
using System.Collections.Generic;
using LFG.Recensioni;

namespace LFG.Recensioni;

public abstract class RecensioneWithNavigationPropertiesBase
{
    public Recensione Recensione { get; set; } = null!;
    public Cliente? Cliente { get; set; }

    public Prodotto? Prodotto { get; set; }
}