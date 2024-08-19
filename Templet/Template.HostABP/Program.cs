using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Template.HostABP;
using Volo.Abp;

var builder = Host.CreateDefaultBuilder(args);
builder.ConfigureServices(services =>
{
    //services.AddHostedService<MyConsoleAppHostedService>();
    services.AddApplicationAsync<AppModule>(options =>
    {
        options.Services.ReplaceConfiguration(services.GetConfiguration());
        //options.Services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog());
    });
}).AddAppSettingsSecretsJson().UseAutofac().UseConsoleLifetime();

var host = builder.Build();
await host.Services.GetRequiredService<IAbpApplicationWithExternalServiceProvider>().InitializeAsync(host.Services);
await host.RunAsync();