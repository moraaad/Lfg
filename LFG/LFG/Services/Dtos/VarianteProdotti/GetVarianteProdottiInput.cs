using Volo.Abp.Application.Dtos;
using System;

namespace LFG.VarianteProdotti;

public abstract class GetVarianteProdottiInputBase : PagedAndSortedResultRequestDto
{
    public string? FilterText { get; set; }

    public string? Taglia { get; set; }

    public string? Colore { get; set; }

    public string? Materiale { get; set; }

    public string? UrlImmagine { get; set; }

    public int? QtaMagazzinoMin { get; set; }

    public int? QtaMagazzinoMax { get; set; }

    public GetVarianteProdottiInputBase()
    {
    }
}