using LFG.Ordini;
using System;
using Volo.Abp.Application.Dtos;
using System.Collections.Generic;

namespace LFG.Pagamenti;

public abstract class PagamentoWithNavigationPropertiesDtoBase
{
    public PagamentoDto Pagamento { get; set; } = null!;
    public OrdineDto? Ordine { get; set; }
}