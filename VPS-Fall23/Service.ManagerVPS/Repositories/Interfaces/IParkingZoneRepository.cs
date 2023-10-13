using Service.ManagerVPS.Models;

namespace Service.ManagerVPS.Repositories.Interfaces;

public interface IParkingZoneRepository : IVpsRepository<ParkingZone>
{
    ParkingZone? GetParkingZoneById(Guid id);
    
    IQueryable<ParkingZone> GetByCommuneId(Guid communeId);
    
    IQueryable<ParkingZone> GetByCityId(Guid cityId);
    
    IQueryable<ParkingZone> GetByDistrictId(Guid districtId);
}