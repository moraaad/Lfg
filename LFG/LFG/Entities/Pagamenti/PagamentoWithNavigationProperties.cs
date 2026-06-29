using LFG.Ordini;
using System;
using System.Collections.Generic;
using LFG.Pagamenti;

namespace LFG.Pagamenti;

public abstract class PagamentoWithNavigationPropertiesBase
{
    public Pagamento Pagamento { get; set; } = null!;
    public Ordine? Ordine { get; set; }
}