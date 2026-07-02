using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace LFG.ImmagineVarianti;

public abstract class ImmagineVarianteCreateDtoBase
{
    public Guid VarianteProdottoId { get; set; }

    [Required]
    [StringLength(ImmagineVarianteConsts.UrlMaxLength)]
    public string Url { get; set; } = null!;
    public int Ordine { get; set; }
}