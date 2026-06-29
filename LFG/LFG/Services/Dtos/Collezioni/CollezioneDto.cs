using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace LFG.Collezioni;

public abstract class CollezioneDtoBase : FullAuditedEntityDto<Guid>, IHasConcurrencyStamp
{
    public string Nome { get; set; } = null!;
    public string Stagione { get; set; } = null!;
    public DateTime Anno { get; set; }

    public string Sezione { get; set; } = null!;
    public string ConcurrencyStamp { get; set; } = null!;
}