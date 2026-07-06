using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace LFG.Sconti;

public abstract class ScontoDtoBase : FullAuditedEntityDto<Guid>, IHasConcurrencyStamp
{
    public string Codice { get; set; } = null!;
    public string? Tipo { get; set; }

    public decimal Valore { get; set; }

    public int? LimiteUtilizzi { get; set; }

    public DateTime ValidoDal { get; set; }

    public DateTime ValidoAl { get; set; }

    public string Sezione { get; set; } = null!;
    public string ConcurrencyStamp { get; set; } = null!;
}