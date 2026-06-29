using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace LFG.Pagamenti;

public abstract class PagamentoDtoBase : FullAuditedEntityDto<Guid>, IHasConcurrencyStamp
{
    public string? Metodo { get; set; }

    public string? Stato { get; set; }

    public decimal Importo { get; set; }

    public DateTime DataPagamento { get; set; }

    public string IdTransazione { get; set; } = null!;
    public Guid? OrdineId { get; set; }

    public string ConcurrencyStamp { get; set; } = null!;
}