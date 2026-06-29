using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace LFG.ElementoListe;

public abstract class ElementoListaUpdateDtoBase : IHasConcurrencyStamp
{
    public DateTime DataAggiunta { get; set; }

    public Guid VarianteProdottoId { get; set; }

    public Guid? ListaDesideriId { get; set; }

    public string ConcurrencyStamp { get; set; } = null!;
}