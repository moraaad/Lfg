using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace LFG.Ordini;

public abstract class OrdineDtoBase : FullAuditedEntityDto<Guid>, IHasConcurrencyStamp
{
    public DateTime DataOrdine { get; set; }

    public string? Stato { get; set; }

    public decimal ImportoTotale { get; set; }

    public string? IndSpedizione { get; set; }

    public string? MetodoPagamento { get; set; }

    public Guid? ClienteId { get; set; }

    public Guid? ScontoId { get; set; }

    public Guid? IndirizzoId { get; set; }

    public string ConcurrencyStamp { get; set; } = null!;
}