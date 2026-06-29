using LFG.Collezioni;
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
    protected IRepository<Collezione, Guid> _collezioneRepository;

    public ProdottoManagerBase(IProdottoRepository prodottoRepository, IRepository<Collezione, Guid> collezioneRepository)
    {
        _prodottoRepository = prodottoRepository;
        _collezioneRepository = collezioneRepository;
    }

    public virtual async Task<Prodotto> CreateAsync(List<Guid> collezionesIds, Guid? categoriaId, string nome, string prezzo, string sezione, string? descrizione = null, string? codiceSku = null)
    {
        Check.NotNullOrWhiteSpace(nome, nameof(nome));
        Check.Length(nome, nameof(nome), ProdottoConsts.NomeMaxLength, ProdottoConsts.NomeMinLength);
        Check.NotNullOrWhiteSpace(prezzo, nameof(prezzo));
        Check.NotNullOrWhiteSpace(sezione, nameof(sezione));
        Check.Length(sezione, nameof(sezione), ProdottoConsts.SezioneMaxLength, ProdottoConsts.SezioneMinLength);
        Check.Length(descrizione, nameof(descrizione), ProdottoConsts.DescrizioneMaxLength);
        Check.Length(codiceSku, nameof(codiceSku), ProdottoConsts.CodiceSkuMaxLength);
        var prodotto = new Prodotto(GuidGenerator.Create(), categoriaId, nome, prezzo, sezione, descrizione, codiceSku);
        await SetCollezionesAsync(prodotto, collezionesIds);
        return await _prodottoRepository.InsertAsync(prodotto);
    }

    public virtual async Task<Prodotto> UpdateAsync(Guid id, List<Guid> collezionesIds, Guid? categoriaId, string nome, string prezzo, string sezione, string? descrizione = null, string? codiceSku = null, [CanBeNull] string? concurrencyStamp = null)
    {
        Check.NotNullOrWhiteSpace(nome, nameof(nome));
        Check.Length(nome, nameof(nome), ProdottoConsts.NomeMaxLength, ProdottoConsts.NomeMinLength);
        Check.NotNullOrWhiteSpace(prezzo, nameof(prezzo));
        Check.NotNullOrWhiteSpace(sezione, nameof(sezione));
        Check.Length(sezione, nameof(sezione), ProdottoConsts.SezioneMaxLength, ProdottoConsts.SezioneMinLength);
        Check.Length(descrizione, nameof(descrizione), ProdottoConsts.DescrizioneMaxLength);
        Check.Length(codiceSku, nameof(codiceSku), ProdottoConsts.CodiceSkuMaxLength);
        var queryable = await _prodottoRepository.WithDetailsAsync(x => x.Colleziones);
        var query = queryable.Where(x => x.Id == id);
        var prodotto = await AsyncExecuter.FirstOrDefaultAsync(query);
        prodotto.CategoriaId = categoriaId;
        prodotto.Nome = nome;
        prodotto.Prezzo = prezzo;
        prodotto.Sezione = sezione;
        prodotto.Descrizione = descrizione;
        prodotto.CodiceSku = codiceSku;
        await SetCollezionesAsync(prodotto, collezionesIds);
        prodotto.SetConcurrencyStampIfNotNull(concurrencyStamp);
        return await _prodottoRepository.UpdateAsync(prodotto);
    }

    private async Task SetCollezionesAsync(Prodotto prodotto, List<Guid> collezioneIds)
    {
        if (collezioneIds == null || !collezioneIds.Any())
        {
            prodotto.RemoveAllColleziones();
            return;
        }

        var query = (await _collezioneRepository.GetQueryableAsync()).Where(x => collezioneIds.Contains(x.Id)).Select(x => x.Id);
        var collezioneIdsInDb = await AsyncExecuter.ToListAsync(query);
        if (!collezioneIdsInDb.Any())
        {
            return;
        }

        prodotto.RemoveAllCollezionesExceptGivenIds(collezioneIdsInDb);
        foreach (var collezioneId in collezioneIdsInDb)
        {
            prodotto.AddToColleziones(collezioneId);
        }
    }
}