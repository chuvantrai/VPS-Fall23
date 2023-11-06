using Quartz;

namespace Service.ManagerVPS.Extensions.StaticLogic.Scheduler;

public class DeleteParkingZoneJob : IJob
{
   

    public DeleteParkingZoneJob()
    {

    }

    public async Task Execute(IJobExecutionContext context)
    {
        Console.WriteLine("Delete job start running!");

    }
}