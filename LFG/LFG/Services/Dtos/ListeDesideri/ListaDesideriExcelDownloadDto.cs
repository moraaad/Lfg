using Volo.Abp.Application.Dtos;
using System;

namespace LFG.ListeDesideri;

public abstract class ListaDesideriExcelDownloadDtoBase
{
    public string DownloadToken { get; set; } = null!;
    public string? FilterText { get; set; }

    public DateTime? DataCreazioneMin { get; set; }

    public DateTime? DataCreazioneMax { get; set; }

    public string? NomeLista { get; set; }

    public Guid? ClienteId { get; set; }

    public ListaDesideriExcelDownloadDtoBase()
    {
    }
}