using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.WorkerVPS.Models.ParkingZoneJob
{
    internal class AutoDeleteDto
    {
        public Guid ParkingZoneId { get; set; }
        public DateTime DeleteAt { get; set; }
    }
}
