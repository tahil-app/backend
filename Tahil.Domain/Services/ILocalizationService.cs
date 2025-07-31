using Tahil.Domain.Localization;

namespace Tahil.Domain.Services;

public interface ILocalizationService
{
    LocalizedStrings Locales { get; }
    string this[string key] { get; }
    string Get(string key, params object[] args);
}