using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace LFG.VarianteProdotti;

public abstract class VarianteProdottoDtoBase : FullAuditedEntityDto<Guid>
{
    public Guid ProdottoId { get; set; }

    public string? Taglia { get; set; }

    public string? Colore { get; set; }

    public string? Materiale { get; set; }

    public string? UrlImmagine { get; set; }

    public int QtaMagazzino { get; set; }
}