using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Data;

namespace LFG.Categorie;

public abstract class CategoriaManagerBase : DomainService
{
    protected ICategoriaRepository _categoriaRepository;

    public CategoriaManagerBase(ICategoriaRepository categoriaRepository)
    {
        _categoriaRepository = categoriaRepository;
    }

    public virtual async Task<Categoria> CreateAsync(string nome, string sezione, string? descrizione = null)
    {
        Check.NotNullOrWhiteSpace(nome, nameof(nome));
        Check.Length(nome, nameof(nome), CategoriaConsts.NomeMaxLength, CategoriaConsts.NomeMinLength);
        Check.NotNullOrWhiteSpace(sezione, nameof(sezione));
        Check.Length(sezione, nameof(sezione), CategoriaConsts.SezioneMaxLength, CategoriaConsts.SezioneMinLength);
        Check.Length(descrizione, nameof(descrizione), CategoriaConsts.DescrizioneMaxLength);
        var categoria = new Categoria(GuidGenerator.Create(), nome, sezione, descrizione);
        return await _categoriaRepository.InsertAsync(categoria);
    }

    public virtual async Task<Categoria> UpdateAsync(Guid id, string nome, string sezione, string? descrizione = null, [CanBeNull] string? concurrencyStamp = null)
    {
        Check.NotNullOrWhiteSpace(nome, nameof(nome));
        Check.Length(nome, nameof(nome), CategoriaConsts.NomeMaxLength, CategoriaConsts.NomeMinLength);
        Check.NotNullOrWhiteSpace(sezione, nameof(sezione));
        Check.Length(sezione, nameof(sezione), CategoriaConsts.SezioneMaxLength, CategoriaConsts.SezioneMinLength);
        Check.Length(descrizione, nameof(descrizione), CategoriaConsts.DescrizioneMaxLength);
        var categoria = await _categoriaRepository.GetAsync(id);
        categoria.Nome = nome;
        categoria.Sezione = sezione;
        categoria.Descrizione = descrizione;
        categoria.SetConcurrencyStampIfNotNull(concurrencyStamp);
        return await _categoriaRepository.UpdateAsync(categoria);
    }
}