using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace LFG.Categorie;

public abstract class CategoriaUpdateDtoBase : IHasConcurrencyStamp
{
    [Required]
    [StringLength(CategoriaConsts.NomeMaxLength, MinimumLength = CategoriaConsts.NomeMinLength)]
    public string Nome { get; set; } = null!;
    [StringLength(CategoriaConsts.DescrizioneMaxLength)]
    public string? Descrizione { get; set; }

    [Required]
    [StringLength(CategoriaConsts.SezioneMaxLength, MinimumLength = CategoriaConsts.SezioneMinLength)]
    public string Sezione { get; set; } = null!;
    public string ConcurrencyStamp { get; set; } = null!;
}