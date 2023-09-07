namespace Service.Schedule;

public class RepeatingService : BackgroundService
{
    private readonly PeriodicTimer _timer = new(TimeSpan.FromMilliseconds(1000)); // 1s

    public RepeatingService()
    {
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (await _timer.WaitForNextTickAsync(stoppingToken) && !stoppingToken.IsCancellationRequested)
        {
            // await DoWorkAsync();
            await CheckRoleUserVip();
        }
        
    }

    private static async Task DoWorkAsync()
    {
        Console.WriteLine(DateTime.Now.ToString("o"));
    }

    private static async Task CheckRoleUserVip()
    {
        if (DateTime.Now.Hour == 0)
        {
        }
    }
}