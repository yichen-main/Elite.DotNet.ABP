using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System.Collections.Concurrent;
using System.Reflection;
using System.Text.Json;
using Volo.Abp.DependencyInjection;

namespace Console.Localization.Foundations;

[Dependency(ServiceLifetime.Singleton)]
public class OptimizedJsonStringLocalizerFactory : IStringLocalizerFactory
{
    readonly ConcurrentDictionary<string, Dictionary<string, string?>> Localizations = [];

    public OptimizedJsonStringLocalizerFactory(Assembly assembly)
    {
        foreach (var resourceName in assembly.GetManifestResourceNames())
        {
            if (resourceName.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
            {
                using var stream = assembly.GetManifestResourceStream(resourceName);
                if (stream is not null)
                {
                    using StreamReader reader = new(stream);
                    var document = JsonDocument.Parse(reader.ReadToEnd());
                    var culture = document.RootElement.GetProperty("culture").GetString();
                    var texts = document.RootElement.GetProperty("texts").EnumerateObject()
                        .ToDictionary(x => x.Name, x => x.Value.GetString());
                    if (culture is not null) Localizations.AddOrUpdate(culture, texts, (key, oldValue) =>
                    {
                        foreach (var (textKey, textValue) in texts)
                        {
                            oldValue[textKey] = textValue;
                        }
                        return oldValue;
                    });
                }
            }
        }
    }
    public IStringLocalizer Create(Type resourceSource)
    {
        return new OptimizedJsonStringLocalizer(Localizations);
    }
    public IStringLocalizer Create(string baseName, string location)
    {
        return new OptimizedJsonStringLocalizer(Localizations);
    }
}