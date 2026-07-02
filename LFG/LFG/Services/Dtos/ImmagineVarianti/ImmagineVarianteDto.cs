using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace LFG.ImmagineVarianti;

public abstract class ImmagineVarianteDtoBase : FullAuditedEntityDto<Guid>, IHasConcurrencyStamp
{
    public Guid VarianteProdottoId { get; set; }

    public string Url { get; set; } = null!;
    public int Ordine { get; set; }

    public string ConcurrencyStamp { get; set; } = null!;
}