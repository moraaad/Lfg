using Volo.Abp.Application.Dtos;
using System;

namespace LFG.Ordini;

public abstract class OrdineExcelDownloadDtoBase
{
    public string DownloadToken { get; set; } = null!;
    public string? FilterText { get; set; }

    public DateTime? DataOrdineMin { get; set; }

    public DateTime? DataOrdineMax { get; set; }

    public string? Stato { get; set; }

    public decimal? ImportoTotaleMin { get; set; }

    public decimal? ImportoTotaleMax { get; set; }

    public string? IndSpedizione { get; set; }

    public string? MetodoPagamento { get; set; }

    public Guid? ClienteId { get; set; }

    public Guid? ScontoId { get; set; }

    public Guid? IndirizzoId { get; set; }

    public OrdineExcelDownloadDtoBase()
    {
    }
}