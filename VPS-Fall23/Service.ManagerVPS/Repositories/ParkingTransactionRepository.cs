using Microsoft.EntityFrameworkCore;
using Service.ManagerVPS.Models;
using Service.ManagerVPS.Repositories.Interfaces;

namespace Service.ManagerVPS.Repositories
{
    public class ParkingTransactionRepository : VpsRepository<ParkingTransaction>, IParkingTransactionRepository
    {
        public ParkingTransactionRepository(FALL23_SWP490_G14Context fALL23_SWP490_G14Context)
            : base(fALL23_SWP490_G14Context)
        {

        }

        public async Task<int> GetRemainingSlot(Guid parkingZoneId)
        {
            return await GetRemainingSlot(parkingZoneId, DateTime.Now);
        }

        public async Task<int> GetRemainingSlot(Guid parkingZoneId, DateTime checkAt)
        {
            return await this.entities
                 .Where(p => p.ParkingZoneId == parkingZoneId
                 && p.CheckinAt <= checkAt && p.CheckoutAt >= checkAt)
                 .CountAsync();
        }

        public bool IsAlreadyBooking(ParkingTransaction parkingTransaction)
        {
            var transFound = this.entities
                .FirstOrDefault(pt => pt.ParkingZoneId == parkingTransaction.ParkingZoneId
                && pt.LicensePlate.Equals(parkingTransaction.LicensePlate)
                );

            throw new NotImplementedException();
        }

    }
}
