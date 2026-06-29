using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Data;

namespace LFG.Collezioni;

public abstract class CollezioneManagerBase : DomainService
{
    protected ICollezioneRepository _collezioneRepository;

    public CollezioneManagerBase(ICollezioneRepository collezioneRepository)
    {
        _collezioneRepository = collezioneRepository;
    }

    public virtual async Task<Collezione> CreateAsync(string nome, string stagione, DateTime anno, string sezione)
    {
        Check.NotNullOrWhiteSpace(nome, nameof(nome));
        Check.Length(nome, nameof(nome), CollezioneConsts.NomeMaxLength, CollezioneConsts.NomeMinLength);
        Check.NotNullOrWhiteSpace(stagione, nameof(stagione));
        Check.Length(stagione, nameof(stagione), CollezioneConsts.StagioneMaxLength, CollezioneConsts.StagioneMinLength);
        Check.NotNull(anno, nameof(anno));
        Check.NotNullOrWhiteSpace(sezione, nameof(sezione));
        Check.Length(sezione, nameof(sezione), CollezioneConsts.SezioneMaxLength, CollezioneConsts.SezioneMinLength);
        var collezione = new Collezione(GuidGenerator.Create(), nome, stagione, anno, sezione);
        return await _collezioneRepository.InsertAsync(collezione);
    }

    public virtual async Task<Collezione> UpdateAsync(Guid id, string nome, string stagione, DateTime anno, string sezione, [CanBeNull] string? concurrencyStamp = null)
    {
        Check.NotNullOrWhiteSpace(nome, nameof(nome));
        Check.Length(nome, nameof(nome), CollezioneConsts.NomeMaxLength, CollezioneConsts.NomeMinLength);
        Check.NotNullOrWhiteSpace(stagione, nameof(stagione));
        Check.Length(stagione, nameof(stagione), CollezioneConsts.StagioneMaxLength, CollezioneConsts.StagioneMinLength);
        Check.NotNull(anno, nameof(anno));
        Check.NotNullOrWhiteSpace(sezione, nameof(sezione));
        Check.Length(sezione, nameof(sezione), CollezioneConsts.SezioneMaxLength, CollezioneConsts.SezioneMinLength);
        var collezione = await _collezioneRepository.GetAsync(id);
        collezione.Nome = nome;
        collezione.Stagione = stagione;
        collezione.Anno = anno;
        collezione.Sezione = sezione;
        collezione.SetConcurrencyStampIfNotNull(concurrencyStamp);
        return await _collezioneRepository.UpdateAsync(collezione);
    }
}