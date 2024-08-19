using Microsoft.Extensions.Hosting;

namespace Console.Localization;
public class MyBackgroundService : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            // 執行後台任務
            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }
    }
}