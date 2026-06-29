using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace LFG.Collezioni;

public abstract class CollezioneCreateDtoBase
{
    [Required]
    [StringLength(CollezioneConsts.NomeMaxLength, MinimumLength = CollezioneConsts.NomeMinLength)]
    public string Nome { get; set; } = null!;
    [Required]
    [StringLength(CollezioneConsts.StagioneMaxLength, MinimumLength = CollezioneConsts.StagioneMinLength)]
    public string Stagione { get; set; } = null!;
    public DateTime Anno { get; set; }

    [Required]
    [StringLength(CollezioneConsts.SezioneMaxLength, MinimumLength = CollezioneConsts.SezioneMinLength)]
    public string Sezione { get; set; } = null!;
}