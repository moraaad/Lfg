using Volo.Abp.Application.Dtos;
using System;

namespace LFG.Recensioni;

public abstract class RecensioneExcelDownloadDtoBase
{
    public string DownloadToken { get; set; } = null!;
    public string? FilterText { get; set; }

    public int? ValutazioneMin { get; set; }

    public int? ValutazioneMax { get; set; }

    public string? Commento { get; set; }

    public DateTime? DataRecensioneMin { get; set; }

    public DateTime? DataRecensioneMax { get; set; }

    public Guid? ClienteId { get; set; }

    public Guid? ProdottoId { get; set; }

    public RecensioneExcelDownloadDtoBase()
    {
    }
}