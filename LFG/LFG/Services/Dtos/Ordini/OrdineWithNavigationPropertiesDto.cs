using LFG.Clienti;
using LFG.Sconti;
using LFG.Indirizzi;
using System;
using Volo.Abp.Application.Dtos;
using System.Collections.Generic;

namespace LFG.Ordini;

public abstract class OrdineWithNavigationPropertiesDtoBase
{
    public OrdineDto Ordine { get; set; } = null!;
    public ClienteDto? Cliente { get; set; }

    public ScontoDto? Sconto { get; set; }

    public IndirizzoDto? Indirizzo { get; set; }
}