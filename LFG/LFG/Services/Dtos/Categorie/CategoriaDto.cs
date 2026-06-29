using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace LFG.Categorie;

public abstract class CategoriaDtoBase : FullAuditedEntityDto<Guid>, IHasConcurrencyStamp
{
    public string Nome { get; set; } = null!;
    public string? Descrizione { get; set; }

    public string Sezione { get; set; } = null!;
    public string ConcurrencyStamp { get; set; } = null!;
}