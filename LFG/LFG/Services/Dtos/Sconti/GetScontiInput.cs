using Volo.Abp.Application.Dtos;
using System;

namespace LFG.Sconti;

public abstract class GetScontiInputBase : PagedAndSortedResultRequestDto
{
    public string? FilterText { get; set; }

    public string? Codice { get; set; }

    public string? Tipo { get; set; }

    public decimal? ValoreMin { get; set; }

    public decimal? ValoreMax { get; set; }

    public int? LimiteUtilizziMin { get; set; }

    public int? LimiteUtilizziMax { get; set; }

    public DateTime? ValidoDalMin { get; set; }

    public DateTime? ValidoDalMax { get; set; }

    public DateTime? ValidoAlMin { get; set; }

    public DateTime? ValidoAlMax { get; set; }

    public string? Sezione { get; set; }

    public GetScontiInputBase()
    {
    }
}