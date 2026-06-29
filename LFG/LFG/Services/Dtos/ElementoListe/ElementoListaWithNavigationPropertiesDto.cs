using LFG.VarianteProdotti;
using LFG.ListeDesideri;
using System;
using Volo.Abp.Application.Dtos;
using System.Collections.Generic;

namespace LFG.ElementoListe;

public abstract class ElementoListaWithNavigationPropertiesDtoBase
{
    public ElementoListaDto ElementoLista { get; set; } = null!;
    public VarianteProdottoDto VarianteProdotto { get; set; } = null!;
    public ListaDesideriDto? ListaDesideri { get; set; }
}