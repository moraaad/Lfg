using Volo.Abp.Application.Dtos;
using System;

namespace LFG.RigaOrdini;

public abstract class GetRigaOrdiniInputBase : PagedAndSortedResultRequestDto
{
    public string? FilterText { get; set; }

    public int? QuantitaMin { get; set; }

    public int? QuantitaMax { get; set; }

    public decimal? PrezzoUnitarioMin { get; set; }

    public decimal? PrezzoUnitarioMax { get; set; }

    public Guid? OrdineId { get; set; }

    public Guid? VarianteProdottoId { get; set; }

    public GetRigaOrdiniInputBase()
    {
    }
}