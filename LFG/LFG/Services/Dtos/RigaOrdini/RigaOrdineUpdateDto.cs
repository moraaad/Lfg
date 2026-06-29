using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace LFG.RigaOrdini;

public abstract class RigaOrdineUpdateDtoBase : IHasConcurrencyStamp
{
    [Required]
    [Range(RigaOrdineConsts.QuantitaMinLength, RigaOrdineConsts.QuantitaMaxLength)]
    public int Quantita { get; set; }

    [Required]
    public decimal PrezzoUnitario { get; set; }

    public Guid? OrdineId { get; set; }

    public Guid? VarianteProdottoId { get; set; }

    public string ConcurrencyStamp { get; set; } = null!;
}