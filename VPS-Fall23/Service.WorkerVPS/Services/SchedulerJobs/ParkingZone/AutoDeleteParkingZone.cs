using Quartz;
using Service.WorkerVPS.ExternalClients;

namespace Service.WorkerVPS.Scheduler.ParkingZoneSchedulerJobs;

public class AutoDeleteParkingZone : IJob
{
    readonly VpsClient vpsClient;
    
    public AutoDeleteParkingZone(IConfiguration configuration)
    {
        vpsClient = new(configuration.GetValue<string>("VpsClientBaseUrl"));
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var parkingZoneId = context.MergedJobDataMap.GetGuid("parkingZoneId");
        await vpsClient.DeleteParkingZone(parkingZoneId);
    }
}