using Microsoft.Extensions.Localization;
using System.Reflection;
using Tahil.Domain.Localization;

namespace Tahil.Application.Services;

public class LocalizationService : ILocalizationService
{
    private readonly IStringLocalizer _localizer;

    public LocalizationService(IStringLocalizerFactory factory)
    {
        var markerType = Type.GetType("Tahil.API.Resources.LocalizationResourceMarker, Tahil.API")!;
        var assemblyName = new AssemblyName(markerType.Assembly.FullName!);
        _localizer = factory.Create("Locales", assemblyName.Name!);
    }

    public string this[string key] => _localizer[key];

    public LocalizedStrings Locales => new LocalizedStrings(this);

    public string Get(string key, params object[] args)
        => string.Format(_localizer[key], args);
}