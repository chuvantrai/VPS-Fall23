using Microsoft.EntityFrameworkCore;
using Service.ManagerVPS.DTO.Input;
using Service.ManagerVPS.DTO.OtherModels;
using Service.ManagerVPS.Models;
using Service.ManagerVPS.Repositories.Interfaces;

namespace Service.ManagerVPS.Repositories;

public class ParkingZoneRepository : VpsRepository<ParkingZone>, IParkingZoneRepository
{
    public ParkingZoneRepository(FALL23_SWP490_G14Context context) : base(context)
    {
    }

    public List<ParkingZone> GetAllParkingZone()
    {
        var parkingZone = context.ParkingZones.Include(o => o.Owner).ToList();
        return parkingZone;
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

    public PagedList<ParkingZone> GetRequestedParkingZones(QueryStringParameters parameters)
    {
        var requestedParkingZones = entities
            .Where(p => p.IsApprove == null)
            .OrderBy(p => p.SubId);
        return PagedList<ParkingZone>.ToPagedList(requestedParkingZones, parameters.PageNumber,
            parameters.PageSize);
    }
}