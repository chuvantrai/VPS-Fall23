using Microsoft.EntityFrameworkCore;
using Service.ManagerVPS.DTO.OtherModels;
using Service.ManagerVPS.DTO.Output;
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

    public PagedList<ParkingZone> GetOwnerParkingZone(QueryStringParameters parameters, Guid id)
    {
        var parkingZone = entities.Include(o => o.Owner).Where(x => x.OwnerId == id);
        return PagedList<ParkingZone>.ToPagedList(parkingZone, parameters.PageNumber,
            parameters.PageSize);
    }

    public PagedList<ParkingZone> GetOwnerParkingZoneByName(QueryStringParameters parameters, string name, Guid id)
    {
        var parkingZone = entities.Include(o => o.Owner).Where(x => x.Name.Contains(name) && x.OwnerId == id);

        return PagedList<ParkingZone>.ToPagedList(parkingZone, parameters.PageNumber,
            parameters.PageSize);
    }

    public PagedList<ParkingZone> GetParkingZoneByName(QueryStringParameters parameters, string name)
    {
        var parkingZone = entities.Include(o => o.Owner).Where(x => x.Name.Contains(name));
        return PagedList<ParkingZone>.ToPagedList(parkingZone, parameters.PageNumber,
            parameters.PageSize);
    }

    public PagedList<ParkingZone> GetParkingZoneByOwner(QueryStringParameters parameters,
        string owner)
    {
        var parkingZone = entities.Include(o => o.Owner)
            .Where(x => x.Owner.Email == owner);
        return PagedList<ParkingZone>.ToPagedList(parkingZone, parameters.PageNumber,
            parameters.PageSize);
    }

    public List<ParkingZone> GetParkingZoneByOwnerId(string ownerId)
    {
        var list = entities.Where(x => x.OwnerId.ToString().ToLower().Equals(ownerId)).ToList();
        return list;
    }

    public ParkingZone? GetParkingZoneById(Guid id)
    {
        var parkingZone = context.ParkingZones
            .Include(x => x.ParkingZoneAbsents)
            .Include(x => x.Commune)
            .ThenInclude(x => x.District)
            .ThenInclude(x => x.City)
            .FirstOrDefault(x => x.Id.Equals(id));
        return parkingZone;
    }

    public ParkingZoneAndOwnerOutput? GetParkingZoneAndOwnerByParkingZoneId(Guid id)
    {
        var parkingZone = context.ParkingZones
            .Include(x => x.Owner)
            .FirstOrDefault(x => x.Id.Equals(id));
        if (parkingZone == null) return null;

        var numberOfParkingZone =
            context.ParkingZones.Count(x => x.OwnerId.Equals(parkingZone.OwnerId));

        var result = new ParkingZoneAndOwnerOutput
        {
            ParkingZone = parkingZone,
            Owner = parkingZone.Owner,
            NumberOfParkingZones = numberOfParkingZone
        };

        return result;
    }

    public IQueryable<ParkingZone> GetByCityId(Guid cityId)
    {
        return entities
            .Include(p => p.Owner)
            .Include(p => p.Commune)
            .ThenInclude(c => c.District)
            .ThenInclude(d => d.City)
            .Where(p => p.Commune.District.CityId == cityId
                        && p.IsFull == false
                        && p.IsApprove == true
                        && !p.ParkingZoneAbsents.Any(pa => pa.From <= DateTime.Now && pa.To >= DateTime.Now));
    }

    public IQueryable<ParkingZone> GetByCommuneId(Guid communeId)
    {
        return entities.Include(p => p.Owner)
            .Include(p => p.Commune)
            .ThenInclude(c => c.District)
            .ThenInclude(d => d.City)
            .Where(p => p.CommuneId == communeId
                        && p.IsFull == false
                        && p.IsApprove == true
                        && !p.ParkingZoneAbsents.Any(pa => pa.From <= DateTime.Now && pa.To >= DateTime.Now));
    }

    public IQueryable<ParkingZone> GetByDistrictId(Guid districtId)
    {
        return entities.Include(p => p.Owner)
            .Include(p => p.Commune)
            .ThenInclude(c => c.District)
            .ThenInclude(d => d.City)
            .Where(p => p.Commune.DistrictId == districtId
                        && p.IsFull == false
                        && p.IsApprove == true
                        && !p.ParkingZoneAbsents.Any(pa =>
                            pa.From <= DateTime.Now && pa.To >= DateTime.Now));
    }

    public PagedList<ParkingZone> GetRequestedParkingZones(QueryStringParameters parameters)
    {
        var requestedParkingZones = entities
            .Where(p => p.IsApprove == null)
            .OrderBy(p => p.SubId);
        return PagedList<ParkingZone>.ToPagedList(requestedParkingZones, parameters.PageNumber,
            parameters.PageSize);
    }

    public ParkingZone? GetParkingZoneAndAbsentById(Guid parkingZoneId)
    {
        var parkingZone = entities
            .Include(x => x.ParkingZoneAbsents)
            .FirstOrDefault(x => x.Id.Equals(parkingZoneId));
        return parkingZone;
    }

    public IEnumerable<ParkingZone>? GetParkingZoneByArrayParkingZoneId(Guid[]? parkingZoneIds)
    {
        if (parkingZoneIds == null || parkingZoneIds.Length == 0) return null;
        return context.ParkingZones.Include(p => p.Owner)
            .Include(p => p.Commune)
            .ThenInclude(c => c.District)
            .ThenInclude(d => d.City)
            .Where(p => parkingZoneIds.Contains(p.Id) && p.IsApprove == true);
    }
}