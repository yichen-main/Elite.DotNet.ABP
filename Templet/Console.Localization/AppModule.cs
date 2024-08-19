using Console.Localization.Foundations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Console.Localization;

[DependsOn(typeof(AbpAutofacModule))]
public class AppModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHostedService<MyBackgroundService>();

        context.Services.Configure<RequestLocalizationOptions>(options =>
        {
            //配置本地化
            var supportedCultures = new[]
            {
                new CultureInfo("zh-CN"),
                new CultureInfo("en-US"),
                new CultureInfo("zh-TW")
                //添加更多支持的語言
            };

            //設置默認文化
            var defaultCulture = "en-US";

            options.DefaultRequestCulture = new RequestCulture(defaultCulture);
            options.SupportedCultures = supportedCultures;
            options.SupportedUICultures = supportedCultures;
            options.RequestCultureProviders =
            [
                new RequestDialectProvider(defaultCulture, supportedCultures),
                new AcceptLanguageHeaderRequestCultureProvider()
            ];
        });
    }
}