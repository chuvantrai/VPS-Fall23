using Service.ManagerVPS.DTO.Input;
using Service.ManagerVPS.Models;

namespace Service.ManagerVPS.Repositories.Interfaces
{
    public interface IParkingTransactionRepository : IVpsRepository<ParkingTransaction>
    {
        Task<string> CanLicensePlateCheckin(string licenseplate, DateTime checkAt, Guid checkBy);
        Task<string> CanLicensePlateCheckout(string licenseplate, DateTime checkAt, Guid checkBy);
        Task<int> GetRemainingSlot(Guid parkingZoneId);
        Task<int> GetRemainingSlot(Guid parkingZoneId, DateTime checkAt);
        bool IsAlreadyBooking(ParkingTransaction parkingTransaction);
        Task<ParkingTransaction?> GetParkingTransactionByIdEmail(Guid id, string email);
        Task<string> CheckLicesePlate(string licenseplate, DateTime checkAt, Guid checkBy);
    }
}