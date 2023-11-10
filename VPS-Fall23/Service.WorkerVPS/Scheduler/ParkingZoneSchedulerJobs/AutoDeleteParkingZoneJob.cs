using Quartz;
using Service.WorkerVPS.ExternalClients;

namespace Service.WorkerVPS.Scheduler.ParkingZoneSchedulerJobs;

public class AutoDeleteParkingZoneJob : IJob
{
    readonly VpsClient vpsClient;
    
    public AutoDeleteParkingZoneJob(IConfiguration configuration)
    {
        vpsClient = new(configuration.GetValue<string>("VpsClientBaseUrl"));
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var parkingZoneId = context.MergedJobDataMap.GetGuid("parkingZoneId");
        await vpsClient.DeleteParkingZone(parkingZoneId);
    }
}