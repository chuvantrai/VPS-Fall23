using Service.ManagerVPS.Models;

namespace Service.ManagerVPS.Repositories.Interfaces
{
    public interface IParkingTransactionRepository : IVpsRepository<ParkingTransaction>
    {
        Task<int> GetRemainingSlot(Guid parkingZoneId);
        Task<int> GetRemainingSlot(Guid parkingZoneId, DateTime checkAt);
        bool IsAlreadyBooking(ParkingTransaction parkingTransaction);
    }
}
