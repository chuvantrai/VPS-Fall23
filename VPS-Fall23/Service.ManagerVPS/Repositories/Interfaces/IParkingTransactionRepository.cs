using Service.ManagerVPS.DTO.Input;
using Service.ManagerVPS.Models;

namespace Service.ManagerVPS.Repositories.Interfaces
{
    public interface IParkingTransactionRepository : IVpsRepository<ParkingTransaction>
    {
        Task<string> CanLicensePlateCheckin(CheckLicensePlate? licensePlateCheckIn);
        Task<string> CanLicensePlateCheckout(CheckLicensePlate? licensePlateCheckOut);
        Task<int> GetRemainingSlot(Guid parkingZoneId);
        Task<int> GetRemainingSlot(Guid parkingZoneId, DateTime checkAt);
        bool IsAlreadyBooking(ParkingTransaction parkingTransaction);
        Task<string> CheckLicesePlate(CheckLicensePlate checkLicensePlate);
        Task<ParkingTransaction?> GetParkingTransactionByIdEmail(Guid id, string email);
    }
}
