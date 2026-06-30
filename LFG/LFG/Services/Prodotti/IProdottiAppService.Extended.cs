using System.Threading.Tasks;

namespace LFG.Prodotti;

public partial interface IProdottiAppService
{
    Task<PrezzoNormalizzazioneResultDto> NormalizzaPrezziAsync();
}
