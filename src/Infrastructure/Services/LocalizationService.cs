using Microsoft.Extensions.Localization;
using DMS.Application.Services;
using DMS.Infrastructure.Resources;

namespace DMS.Infrastructure.Services;

public class LocalizationService : ILocalizationService
{
    private readonly IStringLocalizer<SharedResources> _localizer;

    public LocalizationService(IStringLocalizer<SharedResources> localizer)
    {
        _localizer = localizer;
    }

    public string GetLocalizedString(string key)
    {
        return _localizer[key] ?? string.Empty;
    }

    public Dictionary<string, string> GetLocalizedStrings()
    {
        var localizedStrings = _localizer.GetAllStrings()
            .ToDictionary(s => s.Name, s => s.Value);

        return localizedStrings;
    }
}