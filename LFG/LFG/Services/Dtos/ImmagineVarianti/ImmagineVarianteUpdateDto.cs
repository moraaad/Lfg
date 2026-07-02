using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace LFG.ImmagineVarianti;

public abstract class ImmagineVarianteUpdateDtoBase : IHasConcurrencyStamp
{
    public Guid VarianteProdottoId { get; set; }

    [Required]
    [StringLength(ImmagineVarianteConsts.UrlMaxLength)]
    public string Url { get; set; } = null!;
    public int Ordine { get; set; }

    public string ConcurrencyStamp { get; set; } = null!;
}