using LFG.Clienti;
using LFG.Prodotti;
using System;
using Volo.Abp.Application.Dtos;
using System.Collections.Generic;

namespace LFG.Recensioni;

public abstract class RecensioneWithNavigationPropertiesDtoBase
{
    public RecensioneDto Recensione { get; set; } = null!;
    public ClienteDto? Cliente { get; set; }

    public ProdottoDto? Prodotto { get; set; }
}