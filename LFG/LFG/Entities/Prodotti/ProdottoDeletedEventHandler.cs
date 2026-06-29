using LFG.VarianteProdotti;
using LFG.Prodotti;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;

namespace LFG.Prodotti;

public class ProdottoDeletedEventHandler : ILocalEventHandler<EntityDeletedEventData<Prodotto>>, ITransientDependency
{
    private readonly IVarianteProdottoRepository _varianteProdottoRepository;

    public ProdottoDeletedEventHandler(IVarianteProdottoRepository varianteProdottoRepository)
    {
        _varianteProdottoRepository = varianteProdottoRepository;
    }

    public async Task HandleEventAsync(EntityDeletedEventData<Prodotto> eventData)
    {
        if (eventData.Entity is not ISoftDelete softDeletedEntity)
        {
            return;
        }

        if (!softDeletedEntity.IsDeleted)
        {
            return;
        }

        try
        {
            await _varianteProdottoRepository.DeleteManyAsync(await _varianteProdottoRepository.GetListByProdottoIdAsync(eventData.Entity.Id));
        }
        catch
        {
            //...
        }
    }
}