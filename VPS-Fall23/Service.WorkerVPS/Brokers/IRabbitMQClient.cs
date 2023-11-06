using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.WorkerVPS.Brokers
{
    internal interface IRabbitMQClient
    {
        Task ExecuteAsync(CancellationToken cancellationToken);
    }
}
