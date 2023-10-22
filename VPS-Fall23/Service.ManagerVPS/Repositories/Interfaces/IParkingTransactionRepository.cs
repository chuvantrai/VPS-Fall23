using Service.ManagerVPS.DTO.Input;
using Service.ManagerVPS.Models;

namespace Service.ManagerVPS.Repositories.Interfaces
{
    public interface IParkingTransactionRepository : IVpsRepository<ParkingTransaction>
    {
        Task<string> CanLicensePlateCheckin(CheckLicensePlate? licensePlateCheckIn);
        Task<string> CanLicensePlateCheckout(CheckLicensePlate? licensePlateCheckOut);
        Task<int> GetBookedSlot(Guid parkingZoneId);
        Task<int> GetBookedSlot(Guid parkingZoneId, DateTime checkAt);
        Task<string> CheckLicesePlate(CheckLicensePlate checkLicensePlate);
        Task<bool> IsAlreadyBooking(BookingSlot bookingSlot);
    }
}
