using Quartz;

namespace Service.ManagerVPS.Extensions.StaticLogic.Scheduler;

public class QuartzServices
{
    public IScheduler Scheduler { get; }

    public QuartzServices(ISchedulerFactory schedulerFactory)
    {
        Scheduler = schedulerFactory.GetScheduler().Result;
    }
}