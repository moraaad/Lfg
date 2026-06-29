using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Data;

namespace LFG.ElementoListe;

public abstract class ElementoListaManagerBase : DomainService
{
    protected IElementoListaRepository _elementoListaRepository;

    public ElementoListaManagerBase(IElementoListaRepository elementoListaRepository)
    {
        _elementoListaRepository = elementoListaRepository;
    }

    public virtual async Task<ElementoLista> CreateAsync(Guid varianteProdottoId, Guid? listaDesideriId, DateTime dataAggiunta)
    {
        Check.NotNull(varianteProdottoId, nameof(varianteProdottoId));
        Check.NotNull(dataAggiunta, nameof(dataAggiunta));
        var elementoLista = new ElementoLista(GuidGenerator.Create(), varianteProdottoId, listaDesideriId, dataAggiunta);
        return await _elementoListaRepository.InsertAsync(elementoLista);
    }

    public virtual async Task<ElementoLista> UpdateAsync(Guid id, Guid varianteProdottoId, Guid? listaDesideriId, DateTime dataAggiunta, [CanBeNull] string? concurrencyStamp = null)
    {
        Check.NotNull(varianteProdottoId, nameof(varianteProdottoId));
        Check.NotNull(dataAggiunta, nameof(dataAggiunta));
        var elementoLista = await _elementoListaRepository.GetAsync(id);
        elementoLista.VarianteProdottoId = varianteProdottoId;
        elementoLista.ListaDesideriId = listaDesideriId;
        elementoLista.DataAggiunta = dataAggiunta;
        elementoLista.SetConcurrencyStampIfNotNull(concurrencyStamp);
        return await _elementoListaRepository.UpdateAsync(elementoLista);
    }
}