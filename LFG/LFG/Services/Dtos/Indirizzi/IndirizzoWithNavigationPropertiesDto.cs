using LFG.Clienti;
using System;
using Volo.Abp.Application.Dtos;
using System.Collections.Generic;

namespace LFG.Indirizzi;

public abstract class IndirizzoWithNavigationPropertiesDtoBase
{
    public IndirizzoDto Indirizzo { get; set; } = null!;
    public ClienteDto? Cliente { get; set; }
}