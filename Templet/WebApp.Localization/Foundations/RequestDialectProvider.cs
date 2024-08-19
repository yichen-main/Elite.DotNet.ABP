using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using System.Globalization;

namespace WebApp.Localization.Foundations;
public class RequestDialectProvider(string defaultCulture, CultureInfo[] info) : RequestCultureProvider
{
    public override Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
    {
        if (!httpContext.Request.Headers.TryGetValue("Accept-Language", out var value))
        {
            return Task.FromResult(new ProviderCultureResult(defaultCulture));
        }

        var acceptLanguage = value.ToString();
        if (string.IsNullOrEmpty(acceptLanguage)) Task.FromResult(new ProviderCultureResult(defaultCulture));
        return Task.FromResult(new ProviderCultureResult(acceptLanguage.Split(',')
            .Select(x => x.Split(';')[default]).Select(x => x.Trim())
            .FirstOrDefault(text => info.Any(x => x.Name.Equals(text, StringComparison.OrdinalIgnoreCase))) ?? defaultCulture));
    }
}