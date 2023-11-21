using Quartz;

namespace Service.ManagerVPS.Extensions.StaticLogic.Scheduler;

public class QuartzServices
{
    public IScheduler Scheduler { get; }

    public QuartzServices(ISchedulerFactory schedulerFactory)
    {
        Scheduler = schedulerFactory.GetScheduler().Result;
    }

    public async Task Start()
    {
        await Scheduler.Start();
    }
}