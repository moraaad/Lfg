using Volo.Abp.Application.Dtos;
using System;

namespace LFG.VarianteProdotti;

public class GetVarianteProdottoListInput : PagedAndSortedResultRequestDto
{
    public Guid ProdottoId { get; set; }
}