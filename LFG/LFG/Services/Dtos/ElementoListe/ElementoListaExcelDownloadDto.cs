using Volo.Abp.Application.Dtos;
using System;

namespace LFG.ElementoListe;

public abstract class ElementoListaExcelDownloadDtoBase
{
    public string DownloadToken { get; set; } = null!;
    public string? FilterText { get; set; }

    public DateTime? DataAggiuntaMin { get; set; }

    public DateTime? DataAggiuntaMax { get; set; }

    public Guid? VarianteProdottoId { get; set; }

    public Guid? ListaDesideriId { get; set; }

    public ElementoListaExcelDownloadDtoBase()
    {
    }
}