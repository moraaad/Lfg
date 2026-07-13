using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace LFG.Prodotti;

public abstract class ProdottoUpdateDtoBase : IHasConcurrencyStamp
{
    [Required]
    [StringLength(ProdottoConsts.NomeMaxLength, MinimumLength = ProdottoConsts.NomeMinLength)]
    public string Nome { get; set; } = null!;
    [StringLength(ProdottoConsts.DescrizioneMaxLength)]
    public string? Descrizione { get; set; }

    [Required(AllowEmptyStrings = true)]
    public string Prezzo { get; set; } = null!;
    [StringLength(ProdottoConsts.CodiceSkuMaxLength)]
    public string? CodiceSku { get; set; }

    [Required]
    [StringLength(ProdottoConsts.SezioneMaxLength, MinimumLength = ProdottoConsts.SezioneMinLength)]
    public string Sezione { get; set; } = null!;
    public Guid? CategoriaId { get; set; }

    public Guid? CollezioneId { get; set; }

    public string ConcurrencyStamp { get; set; } = null!;
}