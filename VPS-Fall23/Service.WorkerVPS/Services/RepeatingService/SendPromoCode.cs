namespace Service.WorkerVPS.Services.RepeatingService;

public class SendPromoCode : BackgroundService
{
    private readonly PeriodicTimer _timer = new(TimeSpan.FromMilliseconds(1000)); // 1s

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (await _timer.WaitForNextTickAsync(stoppingToken) && !stoppingToken.IsCancellationRequested)
        {
            await SendPromoCodeForUser();
        }
    }

    private static async Task SendPromoCodeForUser()
    {
        if (DateTime.Now.Minute == 10 && DateTime.Now.Millisecond < 5)
        {
            
            // await Task.Delay(3600000); // 1h
        }
    }
}