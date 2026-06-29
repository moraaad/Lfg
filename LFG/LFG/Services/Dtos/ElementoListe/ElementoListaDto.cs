using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace LFG.ElementoListe;

public abstract class ElementoListaDtoBase : FullAuditedEntityDto<Guid>, IHasConcurrencyStamp
{
    public DateTime DataAggiunta { get; set; }

    public Guid VarianteProdottoId { get; set; }

    public Guid? ListaDesideriId { get; set; }

    public string ConcurrencyStamp { get; set; } = null!;
}