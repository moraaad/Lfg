using Volo.Abp.Application.Dtos;
using System;

namespace LFG.ListeDesideri;

public abstract class GetListeDesideriInputBase : PagedAndSortedResultRequestDto
{
    public string? FilterText { get; set; }

    public DateTime? DataCreazioneMin { get; set; }

    public DateTime? DataCreazioneMax { get; set; }

    public string? NomeLista { get; set; }

    public Guid? ClienteId { get; set; }

    public GetListeDesideriInputBase()
    {
    }
}