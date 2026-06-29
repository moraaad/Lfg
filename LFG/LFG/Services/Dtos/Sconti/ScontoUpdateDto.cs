using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace LFG.Sconti;

public abstract class ScontoUpdateDtoBase : IHasConcurrencyStamp
{
    [Required]
    [StringLength(ScontoConsts.CodiceMaxLength, MinimumLength = ScontoConsts.CodiceMinLength)]
    public string Codice { get; set; } = null!;
    public string? Tipo { get; set; }

    public decimal Valore { get; set; }

    public int? LimiteUtilizzi { get; set; }

    public DateTime ValidoDal { get; set; }

    public DateTime ValidoAl { get; set; }

    public string ConcurrencyStamp { get; set; } = null!;
}