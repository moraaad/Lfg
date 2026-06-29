using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace LFG.Ordini;

public abstract class OrdineCreateDtoBase
{
    public DateTime DataOrdine { get; set; }

    [StringLength(OrdineConsts.StatoMaxLength)]
    public string? Stato { get; set; }

    public decimal ImportoTotale { get; set; }

    [StringLength(OrdineConsts.IndSpedizioneMaxLength)]
    public string? IndSpedizione { get; set; }

    [StringLength(OrdineConsts.MetodoPagamentoMaxLength)]
    public string? MetodoPagamento { get; set; }

    public Guid? ClienteId { get; set; }

    public Guid? ScontoId { get; set; }
}