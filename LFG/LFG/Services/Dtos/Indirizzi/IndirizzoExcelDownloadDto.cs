using Volo.Abp.Application.Dtos;
using System;

namespace LFG.Indirizzi;

public abstract class IndirizzoExcelDownloadDtoBase
{
    public string DownloadToken { get; set; } = null!;
    public string? FilterText { get; set; }

    public string? Paese { get; set; }

    public string? Citta { get; set; }

    public string? Provincia { get; set; }

    public string? Via { get; set; }

    public string? Cap { get; set; }

    public Guid? ClienteId { get; set; }

    public IndirizzoExcelDownloadDtoBase()
    {
    }
}