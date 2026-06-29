using Volo.Abp.Application.Dtos;
using System;

namespace LFG.ElementoListe;

public abstract class GetElementoListeInputBase : PagedAndSortedResultRequestDto
{
    public string? FilterText { get; set; }

    public DateTime? DataAggiuntaMin { get; set; }

    public DateTime? DataAggiuntaMax { get; set; }

    public Guid? VarianteProdottoId { get; set; }

    public Guid? ListaDesideriId { get; set; }

    public GetElementoListeInputBase()
    {
    }
}