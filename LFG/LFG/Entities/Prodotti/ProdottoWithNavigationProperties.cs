using LFG.Categorie;
using LFG.Collezioni;
using System;
using System.Collections.Generic;
using LFG.Prodotti;

namespace LFG.Prodotti;

public abstract class ProdottoWithNavigationPropertiesBase
{
    public Prodotto Prodotto { get; set; } = null!;
    public Categoria? Categoria { get; set; }

    public Collezione? Collezione { get; set; }
}