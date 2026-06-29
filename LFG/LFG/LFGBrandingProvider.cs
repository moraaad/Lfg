using Microsoft.Extensions.Localization;
using LFG.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace LFG;

[Dependency(ReplaceServices = true)]
public class LFGBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<LFGResource> _localizer;

    public LFGBrandingProvider(IStringLocalizer<LFGResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}