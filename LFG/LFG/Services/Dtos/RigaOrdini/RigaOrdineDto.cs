using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace LFG.RigaOrdini;

public abstract class RigaOrdineDtoBase : FullAuditedEntityDto<Guid>, IHasConcurrencyStamp
{
    public int Quantita { get; set; }

    public decimal PrezzoUnitario { get; set; }

    public Guid? OrdineId { get; set; }

    public Guid? VarianteProdottoId { get; set; }

    public string ConcurrencyStamp { get; set; } = null!;
}