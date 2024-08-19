using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System.Collections.Concurrent;
using System.Globalization;
using Volo.Abp.DependencyInjection;

namespace Console.Localization.Ambassador;
public interface ILocalizerLabor
{
    void Write(in string culture, Dictionary<string, string> texts);
}

[Dependency(ServiceLifetime.Singleton)]
file sealed class LocalizerLabor : ILocalizerLabor
{
    public IEnumerable<LocalizedString> Read()
    {
        var culture = CultureInfo.CurrentUICulture.Name;
        return DialectPools.TryGetValue(culture, out var texts) ? texts.Select(x =>
        new LocalizedString(x.Key, x.Value, resourceNotFound: false)) : [];
    }
    public void Write(in string culture, Dictionary<string, string> texts)
    {
        DialectPools.AddOrUpdate(culture, texts, (key, oldValue) =>
        {
            foreach (var (textKey, textValue) in texts)
            {
                oldValue[textKey] = textValue;
            }
            return oldValue;
        });
    }
    ConcurrentDictionary<string, Dictionary<string, string>> DialectPools { get; } = [];
}