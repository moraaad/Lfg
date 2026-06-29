using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace LFG.ListeDesideri;

public abstract class ListaDesideriCreateDtoBase
{
    public DateTime DataCreazione { get; set; }

    [Required]
    [StringLength(ListaDesideriConsts.NomeListaMaxLength, MinimumLength = ListaDesideriConsts.NomeListaMinLength)]
    public string NomeLista { get; set; } = null!;
    public Guid? ClienteId { get; set; }
}