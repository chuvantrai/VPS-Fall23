using Service.ManagerVPS.DTO.Input;
using Service.ManagerVPS.Models;

namespace Service.ManagerVPS.Repositories.Interfaces
{
    public interface IParkingTransactionRepository : IVpsRepository<ParkingTransaction>
    {
        Task<string> CanLicensePlateCheckin(string licenseplate, DateTime checkAt, Guid checkBy);
        Task<string> CanLicensePlateCheckout(string licenseplate, DateTime checkAt, Guid checkBy);
        Task<int> GetBookedSlot(Guid parkingZoneId);
        List<ParkingTransaction> GetBookedSlot(string parkingZoneName, DateTime? checkAt);
        Task<int> GetBookedSlot(Guid parkingZoneId, DateTime checkAt);
        Task<ParkingTransaction?> GetParkingTransactionByIdEmail(Guid id, string email);
        Task<string> CheckLicesePlate(string licenseplate, DateTime checkAt, Guid checkBy);
        Task<string> CheckOutConfirm(string licenseplate, DateTime checkAt, Guid checkBy);
        Task<bool> IsAlreadyBooking(BookingSlot bookingSlot);
    }
}