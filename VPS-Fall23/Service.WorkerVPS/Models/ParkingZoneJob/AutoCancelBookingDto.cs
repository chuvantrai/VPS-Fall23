using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.WorkerVPS.Models.ParkingZoneJob
{
    internal class AutoCancelBookingDto
    {
        public Guid ParkingTransactionId { get; set; }
        public DateTime CancelAt { get; set; }
    }
}
