using Microsoft.EntityFrameworkCore;
using Service.ManagerVPS.DTO.OtherModels;
using Service.ManagerVPS.Models;
using Service.ManagerVPS.Repositories.Interfaces;

namespace Service.ManagerVPS.Repositories;

public class ParkingZoneRepository : VpsRepository<ParkingZone>, IParkingZoneRepository
{
    public ParkingZoneRepository(FALL23_SWP490_G14Context context) : base(context)
    {
    }

    public PagedList<ParkingZone> GetAllParkingZone(QueryStringParameters parameters)
    {
        var parkingZone = entities.Include(o => o.Owner);
        return PagedList<ParkingZone>.ToPagedList(parkingZone, parameters.PageNumber,
            parameters.PageSize);
    }

    public PagedList<ParkingZone> GetParkingZoneByName(QueryStringParameters parameters, string name)
    {
        var parkingZone = entities.Include(o => o.Owner).Where(x => x.Name.Contains(name));
        return PagedList<ParkingZone>.ToPagedList(parkingZone, parameters.PageNumber,
            parameters.PageSize);
    }

    public PagedList<ParkingZone> GetParkingZoneByOwner(QueryStringParameters parameters, string owner)
    {
        var parkingZone = entities.Include(o => o.Owner).Where(x => x.Owner.Email == owner);
        return PagedList<ParkingZone>.ToPagedList(parkingZone, parameters.PageNumber,
            parameters.PageSize);
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