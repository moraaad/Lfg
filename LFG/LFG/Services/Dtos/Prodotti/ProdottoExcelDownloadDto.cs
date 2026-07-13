using Volo.Abp.Application.Dtos;
using System;

namespace LFG.Prodotti;

public abstract class ProdottoExcelDownloadDtoBase
{
    public string DownloadToken { get; set; } = null!;
    public string? FilterText { get; set; }

    public string? Nome { get; set; }

    public string? Descrizione { get; set; }

    public string? Prezzo { get; set; }

    public string? CodiceSku { get; set; }

    public string? Sezione { get; set; }

    public Guid? CategoriaId { get; set; }

    public Guid? CollezioneId { get; set; }

    public ProdottoExcelDownloadDtoBase()
    {
    }
}