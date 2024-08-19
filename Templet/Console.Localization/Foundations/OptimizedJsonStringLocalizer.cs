using Console.Localization.Ambassador;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System.Collections.Concurrent;
using System.Globalization;
using Volo.Abp.DependencyInjection;

namespace Console.Localization.Foundations;

[Dependency(ServiceLifetime.Singleton)]
public class OptimizedJsonStringLocalizer : IStringLocalizer
{
    private readonly ConcurrentDictionary<string, Dictionary<string, string>> _localizedStrings;

    public OptimizedJsonStringLocalizer(ConcurrentDictionary<string, Dictionary<string, string>> localizedStrings)
    {
        _localizedStrings = localizedStrings;
    }

    public LocalizedString this[string name]
    {
        get
        {
            var culture = CultureInfo.CurrentUICulture.Name;
            var value = GetString(culture, name);
            return new LocalizedString(name, value ?? name, resourceNotFound: value == null);
        }
    }

    public LocalizedString this[string name, params object[] arguments]
    {
        get
        {
            var culture = CultureInfo.CurrentUICulture.Name;
            var format = GetString(culture, name);
            var value = string.Format(format ?? name, arguments);
            return new LocalizedString(name, value, resourceNotFound: format == null);
        }
    }

    public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
    {
        var culture = CultureInfo.CurrentUICulture.Name;
        return _localizedStrings.TryGetValue(culture, out var texts) ? texts.Select(kvp =>
        new LocalizedString(kvp.Key, kvp.Value, resourceNotFound: false)) : [];
    }

    private string GetString(string culture, string name)
    {
        if (_localizedStrings.TryGetValue(culture, out var texts))
        {
            texts.TryGetValue(name, out var value);
            return value;
        }
        return null;
    }
    public ILocalizerLabor LocalizerLabor { get; set; }
}