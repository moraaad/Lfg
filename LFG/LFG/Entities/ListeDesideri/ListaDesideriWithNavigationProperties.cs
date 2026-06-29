using LFG.Clienti;
using System;
using System.Collections.Generic;
using LFG.ListeDesideri;

namespace LFG.ListeDesideri;

public abstract class ListaDesideriWithNavigationPropertiesBase
{
    public ListaDesideri ListaDesideri { get; set; } = null!;
    public Cliente? Cliente { get; set; }
}