using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace LFG.ElementoListe;

public abstract class ElementoListaCreateDtoBase
{
    public DateTime DataAggiunta { get; set; }

    public Guid VarianteProdottoId { get; set; }

    public Guid? ListaDesideriId { get; set; }
}