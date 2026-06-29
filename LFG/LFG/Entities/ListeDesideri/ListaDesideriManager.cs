using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Data;

namespace LFG.ListeDesideri;

public abstract class ListaDesideriManagerBase : DomainService
{
    protected IListaDesideriRepository _listaDesideriRepository;

    public ListaDesideriManagerBase(IListaDesideriRepository listaDesideriRepository)
    {
        _listaDesideriRepository = listaDesideriRepository;
    }

    public virtual async Task<ListaDesideri> CreateAsync(Guid? clienteId, DateTime dataCreazione, string nomeLista)
    {
        Check.NotNull(dataCreazione, nameof(dataCreazione));
        Check.NotNullOrWhiteSpace(nomeLista, nameof(nomeLista));
        Check.Length(nomeLista, nameof(nomeLista), ListaDesideriConsts.NomeListaMaxLength, ListaDesideriConsts.NomeListaMinLength);
        var listaDesideri = new ListaDesideri(GuidGenerator.Create(), clienteId, dataCreazione, nomeLista);
        return await _listaDesideriRepository.InsertAsync(listaDesideri);
    }

    public virtual async Task<ListaDesideri> UpdateAsync(Guid id, Guid? clienteId, DateTime dataCreazione, string nomeLista, [CanBeNull] string? concurrencyStamp = null)
    {
        Check.NotNull(dataCreazione, nameof(dataCreazione));
        Check.NotNullOrWhiteSpace(nomeLista, nameof(nomeLista));
        Check.Length(nomeLista, nameof(nomeLista), ListaDesideriConsts.NomeListaMaxLength, ListaDesideriConsts.NomeListaMinLength);
        var listaDesideri = await _listaDesideriRepository.GetAsync(id);
        listaDesideri.ClienteId = clienteId;
        listaDesideri.DataCreazione = dataCreazione;
        listaDesideri.NomeLista = nomeLista;
        listaDesideri.SetConcurrencyStampIfNotNull(concurrencyStamp);
        return await _listaDesideriRepository.UpdateAsync(listaDesideri);
    }
}