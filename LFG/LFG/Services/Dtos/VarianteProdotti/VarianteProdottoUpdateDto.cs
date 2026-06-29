using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace LFG.VarianteProdotti;

public abstract class VarianteProdottoUpdateDtoBase
{
    public Guid ProdottoId { get; set; }

    public string? Taglia { get; set; }

    public string? Colore { get; set; }

    public string? Materiale { get; set; }

    [StringLength(VarianteProdottoConsts.UrlImmagineMaxLength)]
    public string? UrlImmagine { get; set; }

    public int QtaMagazzino { get; set; }
}