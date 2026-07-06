using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Data;

namespace LFG.Sconti;

public abstract class ScontoManagerBase : DomainService
{
    protected IScontoRepository _scontoRepository;

    public ScontoManagerBase(IScontoRepository scontoRepository)
    {
        _scontoRepository = scontoRepository;
    }

    public virtual async Task<Sconto> CreateAsync(string codice, decimal valore, DateTime validoDal, DateTime validoAl, string sezione, string? tipo = null, int? limiteUtilizzi = null)
    {
        Check.NotNullOrWhiteSpace(codice, nameof(codice));
        Check.Length(codice, nameof(codice), ScontoConsts.CodiceMaxLength, ScontoConsts.CodiceMinLength);
        Check.NotNull(validoDal, nameof(validoDal));
        Check.NotNull(validoAl, nameof(validoAl));
        Check.NotNullOrWhiteSpace(sezione, nameof(sezione));
        Check.Length(sezione, nameof(sezione), ScontoConsts.SezioneMaxLength, ScontoConsts.SezioneMinLength);
        var sconto = new Sconto(GuidGenerator.Create(), codice, valore, validoDal, validoAl, sezione, tipo, limiteUtilizzi);
        return await _scontoRepository.InsertAsync(sconto);
    }

    public virtual async Task<Sconto> UpdateAsync(Guid id, string codice, decimal valore, DateTime validoDal, DateTime validoAl, string sezione, string? tipo = null, int? limiteUtilizzi = null, [CanBeNull] string? concurrencyStamp = null)
    {
        Check.NotNullOrWhiteSpace(codice, nameof(codice));
        Check.Length(codice, nameof(codice), ScontoConsts.CodiceMaxLength, ScontoConsts.CodiceMinLength);
        Check.NotNull(validoDal, nameof(validoDal));
        Check.NotNull(validoAl, nameof(validoAl));
        Check.NotNullOrWhiteSpace(sezione, nameof(sezione));
        Check.Length(sezione, nameof(sezione), ScontoConsts.SezioneMaxLength, ScontoConsts.SezioneMinLength);
        var sconto = await _scontoRepository.GetAsync(id);
        sconto.Codice = codice;
        sconto.Valore = valore;
        sconto.ValidoDal = validoDal;
        sconto.ValidoAl = validoAl;
        sconto.Sezione = sezione;
        sconto.Tipo = tipo;
        sconto.LimiteUtilizzi = limiteUtilizzi;
        sconto.SetConcurrencyStampIfNotNull(concurrencyStamp);
        return await _scontoRepository.UpdateAsync(sconto);
    }
}