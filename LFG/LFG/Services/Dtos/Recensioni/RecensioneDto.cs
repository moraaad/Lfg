using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace LFG.Recensioni;

public abstract class RecensioneDtoBase : FullAuditedEntityDto<Guid>, IHasConcurrencyStamp
{
    public int Valutazione { get; set; }

    public string? Commento { get; set; }

    public DateTime DataRecensione { get; set; }

    public Guid? ClienteId { get; set; }

    public Guid? ProdottoId { get; set; }

    public string ConcurrencyStamp { get; set; } = null!;
}