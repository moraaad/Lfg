using Volo.Abp.Application.Dtos;
using System;

namespace LFG.Clienti;

public abstract class GetClientiInputBase : PagedAndSortedResultRequestDto
{
    public string? FilterText { get; set; }

    public string? Nome { get; set; }

    public string? Cognome { get; set; }

    public string? Genere { get; set; }

    public DateTime? DataNascitaMin { get; set; }

    public DateTime? DataNascitaMax { get; set; }

    public string? Email { get; set; }

    public string? Telefono { get; set; }

    public string? Sezione { get; set; }

    public string? Nazionalita { get; set; }

    public Guid? UserId { get; set; }

    public GetClientiInputBase()
    {
    }
}