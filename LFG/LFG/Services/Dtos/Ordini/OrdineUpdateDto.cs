using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace LFG.Ordini;

public abstract class OrdineUpdateDtoBase : IHasConcurrencyStamp
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

    public string ConcurrencyStamp { get; set; } = null!;
}