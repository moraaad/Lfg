using LFG.Clienti;
using System;
using System.Collections.Generic;
using LFG.Indirizzi;

namespace LFG.Indirizzi;

public abstract class IndirizzoWithNavigationPropertiesBase
{
    public Indirizzo Indirizzo { get; set; } = null!;
    public Cliente? Cliente { get; set; }
}