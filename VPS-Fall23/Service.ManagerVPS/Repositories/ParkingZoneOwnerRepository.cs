using Microsoft.EntityFrameworkCore;
using Service.ManagerVPS.DTO.OtherModels;
using Service.ManagerVPS.Models;
using Service.ManagerVPS.Repositories.Interfaces;

namespace Service.ManagerVPS.Repositories;

public class ParkingZoneOwnerRepository : VpsRepository<ParkingZoneOwner>, IParkingZoneOwnerRepository
{
    public ParkingZoneOwnerRepository(FALL23_SWP490_G14Context context) : base(context)
    {
    }

    public PagedList<ParkingZoneOwner> GetAllOwner(QueryStringParameters parameters)
    {
        var list = entities;
        return PagedList<ParkingZoneOwner>.ToPagedList(list, parameters.PageNumber, parameters.PageSize); ;
    }

    public PagedList<ParkingZoneOwner> GetOwnerByEmail(QueryStringParameters parameters, string email)
    {
        var owner = entities.Where(x => x.Email.Contains(email));

        return PagedList<ParkingZoneOwner>.ToPagedList(owner, parameters.PageNumber,
            parameters.PageSize);
    }
}