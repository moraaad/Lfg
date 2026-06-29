using Volo.Abp.Application.Dtos;
using System;

namespace LFG.Collezioni;

public abstract class GetCollezioniInputBase : PagedAndSortedResultRequestDto
{
    public string? FilterText { get; set; }

    public string? Nome { get; set; }

    public string? Stagione { get; set; }

    public DateTime? AnnoMin { get; set; }

    public DateTime? AnnoMax { get; set; }

    public string? Sezione { get; set; }

    public GetCollezioniInputBase()
    {
    }
}