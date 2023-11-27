using Service.ManagerVPS.Models;
using Service.ManagerVPS.Repositories.Interfaces;

namespace Service.ManagerVPS.Repositories;

public class ParkingZoneAbsentRepository : VpsRepository<ParkingZoneAbsent>,
    IParkingZoneAbsentRepository
{
    public ParkingZoneAbsentRepository(FALL23_SWP490_G14Context context) : base(context)
    {
    }

    public Task<IQueryable<ParkingZoneAbsent>> GetByParkingZone(Guid parkingZoneId, DateTime? getFrom)
    {
        if (getFrom == null) getFrom = DateTime.Now;
        getFrom = getFrom.Value.ToLocalTime();
        return Task.FromResult(this.entities.Where(pa => pa.ParkingZoneId == parkingZoneId
        && pa.From >= getFrom.Value.Date));
    }
}