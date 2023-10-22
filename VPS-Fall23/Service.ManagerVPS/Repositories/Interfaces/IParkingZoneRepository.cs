using Service.ManagerVPS.DTO.OtherModels;
using Service.ManagerVPS.Models;

namespace Service.ManagerVPS.Repositories.Interfaces;

public interface IParkingZoneRepository : IVpsRepository<ParkingZone>
{
    List<ParkingZone> GetAllParkingZone();
    
    ParkingZone? GetParkingZoneById(Guid id);
    
    IQueryable<ParkingZone> GetByCommuneId(Guid communeId);
    
    IQueryable<ParkingZone> GetByCityId(Guid cityId);
    
    IQueryable<ParkingZone> GetByDistrictId(Guid districtId);

    PagedList<ParkingZone> GetRequestedParkingZones(QueryStringParameters parameters);

}