using LFG.VarianteProdotti;
using System;
using Volo.Abp.Application.Dtos;
using System.Collections.Generic;

namespace LFG.ImmagineVarianti;

public abstract class ImmagineVarianteWithNavigationPropertiesDtoBase
{
    public ImmagineVarianteDto ImmagineVariante { get; set; } = null!;
    public VarianteProdottoDto? VarianteProdotto { get; set; }
}