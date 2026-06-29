using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace LFG.Indirizzi;

public abstract class IndirizzoCreateDtoBase
{
    public string? Paese { get; set; }

    public string? Citta { get; set; }

    public string? Provincia { get; set; }

    [Required]
    [StringLength(IndirizzoConsts.ViaMaxLength)]
    public string Via { get; set; } = null!;
    [Required]
    [StringLength(IndirizzoConsts.CapMaxLength, MinimumLength = IndirizzoConsts.CapMinLength)]
    public string Cap { get; set; } = null!;
    public Guid? ClienteId { get; set; }
}