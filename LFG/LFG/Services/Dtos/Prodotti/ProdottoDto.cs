using System;
using System.Collections.Generic;
using LFG.VarianteProdotti;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace LFG.Prodotti;

public abstract class ProdottoDtoBase : FullAuditedEntityDto<Guid>, IHasConcurrencyStamp
{
    public string Nome { get; set; } = null!;
    public string? Descrizione { get; set; }

    public string Prezzo { get; set; } = null!;
    public string? CodiceSku { get; set; }

    public string Sezione { get; set; } = null!;
    public Guid? CategoriaId { get; set; }

    public Guid? CollezioneId { get; set; }

    public string ConcurrencyStamp { get; set; } = null!;
    public List<VarianteProdottoDto> VarianteProdotti { get; set; } = new();
}