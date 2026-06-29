using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace LFG.Recensioni;

public abstract class RecensioneCreateDtoBase
{
    [Required]
    [Range(RecensioneConsts.ValutazioneMinLength, RecensioneConsts.ValutazioneMaxLength)]
    public int Valutazione { get; set; }

    [StringLength(RecensioneConsts.CommentoMaxLength)]
    public string? Commento { get; set; }

    public DateTime DataRecensione { get; set; }

    public Guid? ClienteId { get; set; }

    public Guid? ProdottoId { get; set; }
}