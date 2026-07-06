using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace LFG.Sconti;

public abstract class ScontoCreateDtoBase
{
    [Required]
    [StringLength(ScontoConsts.CodiceMaxLength, MinimumLength = ScontoConsts.CodiceMinLength)]
    public string Codice { get; set; } = null!;
    public string? Tipo { get; set; }

    public decimal Valore { get; set; }

    public int? LimiteUtilizzi { get; set; }

    public DateTime ValidoDal { get; set; }

    public DateTime ValidoAl { get; set; }

    [Required]
    [StringLength(ScontoConsts.SezioneMaxLength, MinimumLength = ScontoConsts.SezioneMinLength)]
    public string Sezione { get; set; } = null!;
}