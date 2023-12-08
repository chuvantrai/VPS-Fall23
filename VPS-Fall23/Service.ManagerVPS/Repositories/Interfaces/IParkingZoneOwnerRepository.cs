using Service.ManagerVPS.DTO.OtherModels;
using Service.ManagerVPS.Models;

namespace Service.ManagerVPS.Repositories.Interfaces;

public interface IParkingZoneOwnerRepository : IVpsRepository<ParkingZoneOwner>
{
    PagedList<ParkingZoneOwner> GetAllOwner(QueryStringParameters parameters);
    
    PagedList<ParkingZoneOwner> GetOwnerByEmail(QueryStringParameters parameters, string email);
}