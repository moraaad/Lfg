using LFG.Categorie;
using LFG.Collezioni;
using System;
using Volo.Abp.Application.Dtos;
using System.Collections.Generic;

namespace LFG.Prodotti;

public abstract class ProdottoWithNavigationPropertiesDtoBase
{
    public ProdottoDto Prodotto { get; set; } = null!;
    public CategoriaDto? Categoria { get; set; }

    public CollezioneDto? Collezione { get; set; }
}