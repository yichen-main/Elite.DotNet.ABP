using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Localization;
using System.Globalization;
using System.Reflection;
using Volo.Abp;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;
using WebApp.Localization.Foundations;

namespace WebApp.Localization;

[DependsOn(typeof(AbpAutofacModule))]
public class AppModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        //context.Services.AddHostedService<MyBackgroundService>();
        // 註冊優化後的本地化服務
        context.Services.AddSingleton<IStringLocalizerFactory>(sp =>
           new OptimizedJsonStringLocalizerFactory(Assembly.GetExecutingAssembly()));
        context.Services.AddLocalization();

        context.Services.AddControllers();
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
    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        // 应用初始化逻辑...
    }
}