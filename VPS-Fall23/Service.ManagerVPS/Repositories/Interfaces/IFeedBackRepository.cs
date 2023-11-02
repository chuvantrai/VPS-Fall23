using Service.ManagerVPS.DTO.Input;
using Service.ManagerVPS.Models;

namespace Service.ManagerVPS.Repositories.Interfaces;

public interface IFeedBackRepository : IVpsRepository<Feedback>
{
    Task<dynamic> CreateFeedBack(CreateFeedBackParkingZoneRequest request, ParkingZone parkingZone);
}