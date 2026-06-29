using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace LFG.Indirizzi;

public abstract class IndirizzoDtoBase : FullAuditedEntityDto<Guid>, IHasConcurrencyStamp
{
    public string? Paese { get; set; }

    public string? Citta { get; set; }

    public string? Provincia { get; set; }

    public string Via { get; set; } = null!;
    public string Cap { get; set; } = null!;
    public Guid? ClienteId { get; set; }

    public string ConcurrencyStamp { get; set; } = null!;
}