using LFG.VarianteProdotti;
using LFG.ListeDesideri;
using System;
using System.Collections.Generic;
using LFG.ElementoListe;

namespace LFG.ElementoListe;

public abstract class ElementoListaWithNavigationPropertiesBase
{
    public ElementoLista ElementoLista { get; set; } = null!;
    public VarianteProdotto VarianteProdotto { get; set; } = null!;
    public ListaDesideri? ListaDesideri { get; set; }
}