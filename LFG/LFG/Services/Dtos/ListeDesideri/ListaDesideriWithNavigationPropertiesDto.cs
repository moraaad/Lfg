using LFG.Clienti;
using System;
using Volo.Abp.Application.Dtos;
using System.Collections.Generic;

namespace LFG.ListeDesideri;

public abstract class ListaDesideriWithNavigationPropertiesDtoBase
{
    public ListaDesideriDto ListaDesideri { get; set; } = null!;
    public ClienteDto? Cliente { get; set; }
}