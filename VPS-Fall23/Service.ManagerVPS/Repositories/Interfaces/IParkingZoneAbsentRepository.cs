using Service.ManagerVPS.Models;

namespace Service.ManagerVPS.Repositories.Interfaces;

public interface IParkingZoneAbsentRepository : IVpsRepository<ParkingZoneAbsent>
{
    Task<IQueryable<ParkingZoneAbsent>> GetByParkingZone(Guid parkingZoneId, DateTime? getFrom);
}