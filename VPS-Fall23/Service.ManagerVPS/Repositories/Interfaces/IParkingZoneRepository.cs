using Service.ManagerVPS.DTO.OtherModels;
using Service.ManagerVPS.DTO.Output;
using Service.ManagerVPS.Models;

namespace Service.ManagerVPS.Repositories.Interfaces;

public interface IParkingZoneRepository : IVpsRepository<ParkingZone>
{
    PagedList<ParkingZone> GetAllParkingZone(QueryStringParameters parameters);
    PagedList<ParkingZone> GetParkingZoneByName(QueryStringParameters parameters, string name);
    PagedList<ParkingZone> GetParkingZoneByOwner(QueryStringParameters parameters, string owner);
    ParkingZone? GetParkingZoneById(Guid id);

    ParkingZoneAndOwnerOutput? GetParkingZoneAndOwnerByParkingZoneId(Guid id);
    
    IQueryable<ParkingZone> GetByCommuneId(Guid communeId);
    
    IQueryable<ParkingZone> GetByCityId(Guid cityId);
    
    IQueryable<ParkingZone> GetByDistrictId(Guid districtId);

    PagedList<ParkingZone> GetRequestedParkingZones(QueryStringParameters parameters);
}