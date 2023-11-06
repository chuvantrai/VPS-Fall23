using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.WorkerVPS.ExternalClients
{
    internal class VpsClient
    {
        readonly RestClient restClient;
        const string DeleteParkingZoneUri = "";
        public VpsClient(string baseUrl)
        {
            restClient = new RestClient(baseUrl);
        }
        public 

    }
}
