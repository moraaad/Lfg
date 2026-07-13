using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Data;

namespace LFG.Prodotti;

public abstract class ProdottoManagerBase : DomainService
{
    protected IProdottoRepository _prodottoRepository;

    public ProdottoManagerBase(IProdottoRepository prodottoRepository)
    {
        _prodottoRepository = prodottoRepository;
    }

    public virtual async Task<Prodotto> CreateAsync(Guid? categoriaId, Guid? collezioneId, string nome, string prezzo, string sezione, string? descrizione = null, string? codiceSku = null)
    {
        Check.NotNullOrWhiteSpace(nome, nameof(nome));
        Check.Length(nome, nameof(nome), ProdottoConsts.NomeMaxLength, ProdottoConsts.NomeMinLength);
        Check.NotNullOrWhiteSpace(prezzo, nameof(prezzo));
        Check.NotNullOrWhiteSpace(sezione, nameof(sezione));
        Check.Length(sezione, nameof(sezione), ProdottoConsts.SezioneMaxLength, ProdottoConsts.SezioneMinLength);
        Check.Length(descrizione, nameof(descrizione), ProdottoConsts.DescrizioneMaxLength);
        Check.Length(codiceSku, nameof(codiceSku), ProdottoConsts.CodiceSkuMaxLength);
        var prodotto = new Prodotto(GuidGenerator.Create(), categoriaId, collezioneId, nome, prezzo, sezione, descrizione, codiceSku);
        return await _prodottoRepository.InsertAsync(prodotto);
    }

    public virtual async Task<Prodotto> UpdateAsync(Guid id, Guid? categoriaId, Guid? collezioneId, string nome, string prezzo, string sezione, string? descrizione = null, string? codiceSku = null, [CanBeNull] string? concurrencyStamp = null)
    {
        Check.NotNullOrWhiteSpace(nome, nameof(nome));
        Check.Length(nome, nameof(nome), ProdottoConsts.NomeMaxLength, ProdottoConsts.NomeMinLength);
        Check.NotNullOrWhiteSpace(prezzo, nameof(prezzo));
        Check.NotNullOrWhiteSpace(sezione, nameof(sezione));
        Check.Length(sezione, nameof(sezione), ProdottoConsts.SezioneMaxLength, ProdottoConsts.SezioneMinLength);
        Check.Length(descrizione, nameof(descrizione), ProdottoConsts.DescrizioneMaxLength);
        Check.Length(codiceSku, nameof(codiceSku), ProdottoConsts.CodiceSkuMaxLength);
        var prodotto = await _prodottoRepository.GetAsync(id);
        prodotto.CategoriaId = categoriaId;
        prodotto.CollezioneId = collezioneId;
        prodotto.Nome = nome;
        prodotto.Prezzo = prezzo;
        prodotto.Sezione = sezione;
        prodotto.Descrizione = descrizione;
        prodotto.CodiceSku = codiceSku;
        prodotto.SetConcurrencyStampIfNotNull(concurrencyStamp);
        return await _prodottoRepository.UpdateAsync(prodotto);
    }

    internal async Task<Prodotto> UpdateAsync(Guid id, List<Guid> collezionesIds, Guid? categoriaId, string nome, string v, string sezione, string? descrizione, string? codiceSku, string? concurrencyStamp)
    {
        throw new NotImplementedException();
    }
}