using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace LFG.ListeDesideri;

public abstract class ListaDesideriUpdateDtoBase : IHasConcurrencyStamp
{
    public DateTime DataCreazione { get; set; }

    [Required]
    [StringLength(ListaDesideriConsts.NomeListaMaxLength, MinimumLength = ListaDesideriConsts.NomeListaMinLength)]
    public string NomeLista { get; set; } = null!;
    public Guid? ClienteId { get; set; }

    public string ConcurrencyStamp { get; set; } = null!;
}