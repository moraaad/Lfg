using Volo.Abp.Application.Dtos;
using System;

namespace LFG.ImmagineVarianti;

public abstract class GetImmagineVariantiInputBase : PagedAndSortedResultRequestDto
{
    public string? FilterText { get; set; }

    public Guid? VarianteProdottoId { get; set; }

    public string? Url { get; set; }

    public int? OrdineMin { get; set; }

    public int? OrdineMax { get; set; }

    public GetImmagineVariantiInputBase()
    {
    }
}