using Volo.Abp.Application.Dtos;
using System;

namespace LFG.Pagamenti;

public abstract class GetPagamentiInputBase : PagedAndSortedResultRequestDto
{
    public string? FilterText { get; set; }

    public string? Metodo { get; set; }

    public string? Stato { get; set; }

    public decimal? ImportoMin { get; set; }

    public decimal? ImportoMax { get; set; }

    public DateTime? DataPagamentoMin { get; set; }

    public DateTime? DataPagamentoMax { get; set; }

    public string? IdTransazione { get; set; }

    public Guid? OrdineId { get; set; }

    public GetPagamentiInputBase()
    {
    }
}