using Quartz;
using Service.WorkerVPS.ExternalClients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.WorkerVPS.Services.SchedulerJobs.ParkingZone
{
    internal class AutoCancelBookingTransaction : IJob
    {
        readonly VpsClient vpsClient;

        public AutoCancelBookingTransaction(IConfiguration configuration)
        {
            vpsClient = new(configuration.GetValue<string>("VpsClientBaseUrl"));
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var parkingTransactionId = context.MergedJobDataMap.GetGuid("parkingTransactionId");
            await vpsClient.CancelBookingTransaction(parkingTransactionId);
        }
    }
}
