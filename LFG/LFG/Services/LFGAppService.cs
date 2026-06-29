using Volo.Abp.Application.Services;
using LFG.Localization;

namespace LFG.Services;

/* Inherit your application services from this class. */
public abstract class LFGAppService : ApplicationService
{
    protected LFGAppService()
    {
        LocalizationResource = typeof(LFGResource);
    }
}