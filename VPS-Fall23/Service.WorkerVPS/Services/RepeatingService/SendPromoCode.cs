using Service.WorkerVPS.ExternalClients;

namespace Service.WorkerVPS.Services.RepeatingService;

public class SendPromoCode : BackgroundService
{
    private readonly PeriodicTimer _timer = new(TimeSpan.FromMilliseconds(1000 * 60)); // 1phut
    private readonly VpsClient _vpsClient;

    public SendPromoCode(IConfiguration configuration)
    {
        _vpsClient = new(configuration.GetValue<string>("VpsClientBaseUrl"));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (await _timer.WaitForNextTickAsync(stoppingToken) && !stoppingToken.IsCancellationRequested)
        {
            await SendPromoCodeForUser();
        }
    }

    private async Task SendPromoCodeForUser()
    {
        if (DateTime.Now.Minute == 10) // phut thu 10 se chay
        {
            await _vpsClient.SendNotificationPromoCodeToUser();
        }
    }
}