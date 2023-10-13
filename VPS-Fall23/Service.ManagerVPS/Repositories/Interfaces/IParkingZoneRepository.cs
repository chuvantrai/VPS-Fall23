using Service.ManagerVPS.Models;

namespace Service.ManagerVPS.Repositories.Interfaces;

public interface IParkingZoneRepository : IVpsRepository<ParkingZone>
{
    IQueryable<ParkingZone> GetByCommuneId(Guid communeId);
    IQueryable<ParkingZone> GetByCityId(Guid cityId);
    IQueryable<ParkingZone> GetByDistrictId(Guid districtId);

}