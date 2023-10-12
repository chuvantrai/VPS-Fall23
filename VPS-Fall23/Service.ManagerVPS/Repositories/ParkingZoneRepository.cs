using Microsoft.EntityFrameworkCore;
using Service.ManagerVPS.Models;
using Service.ManagerVPS.Repositories.Interfaces;

namespace Service.ManagerVPS.Repositories;

public class ParkingZoneRepository : VpsRepository<ParkingZone>, IParkingZoneRepository
{
    public ParkingZoneRepository(FALL23_SWP490_G14Context context) : base(context)
    {
    }

    public ParkingZone? GetParkingZoneById(Guid id)
    {
        var parkingZone = context.ParkingZones.FirstOrDefault(x => x.Id.Equals(id));
        return parkingZone;
    }

    public IQueryable<ParkingZone> GetByCityId(Guid cityId)
    {
        return entities
            .Include(p => p.Owner)
            .Include(p => p.Commune)
            .ThenInclude(c => c.District)
            .ThenInclude(d => d.City)
            .Where(p => p.Commune.District.CityId == cityId);
    }

    public IQueryable<ParkingZone> GetByCommuneId(Guid communeId)
    {
        return entities.Include(p => p.Owner)
            .Include(p => p.Commune)
            .ThenInclude(c => c.District)
            .ThenInclude(d => d.City)
            .Where(p => p.CommuneId == communeId);
    }

    public IQueryable<ParkingZone> GetByDistrictId(Guid districtId)
    {
        return entities.Include(p => p.Owner)
            .Include(p => p.Commune)
            .ThenInclude(c => c.District)
            .ThenInclude(d => d.City).Where(p => p.Commune.DistrictId == districtId);
    }
}