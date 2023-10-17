using Service.ManagerVPS.DTO.Input;
using Service.ManagerVPS.Models;

namespace Service.ManagerVPS.Repositories.Interfaces
{
    public interface IParkingTransactionRepository : IVpsRepository<ParkingTransaction>
    {
        Task<string> CanLicensePlateCheckin(string licenseplate, LicensePlateInfo? licensePlateCheckIn);
        Task<string> CanLicensePlateCheckout(string licenseplate, LicensePlateInfo? licensePlateCheckOut);
        Task<int> GetRemainingSlot(Guid parkingZoneId);
        Task<int> GetRemainingSlot(Guid parkingZoneId, DateTime checkAt);
        bool IsAlreadyBooking(ParkingTransaction parkingTransaction);
        Task<string> CheckLicesePlate(string licenseplate, LicensePlateInfo checkLicensePlate);
    }
}
