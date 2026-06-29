using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace LFG.ListeDesideri;

public abstract class ListaDesideriDtoBase : FullAuditedEntityDto<Guid>, IHasConcurrencyStamp
{
    public DateTime DataCreazione { get; set; }

    public string NomeLista { get; set; } = null!;
    public Guid? ClienteId { get; set; }

    public string ConcurrencyStamp { get; set; } = null!;
}