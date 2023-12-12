using Microsoft.EntityFrameworkCore;
using Service.ManagerVPS.DTO.OtherModels;
using Service.ManagerVPS.Models;
using Service.ManagerVPS.Repositories.Interfaces;
using System.Collections.Generic;

namespace Service.ManagerVPS.Repositories;

public class ParkingZoneOwnerRepository : VpsRepository<ParkingZoneOwner>, IParkingZoneOwnerRepository
{
    public ParkingZoneOwnerRepository(FALL23_SWP490_G14Context context) : base(context)
    {
    }

    public PagedList<ParkingZoneOwner> GetAllOwner(QueryStringParameters parameters)
    {
        var list = entities
            .Include(x => x.IdNavigation);
        return PagedList<ParkingZoneOwner>.ToPagedList(list, parameters.PageNumber, parameters.PageSize); ;
    }

    public PagedList<ParkingZoneOwner> GetOwnerByEmail(QueryStringParameters parameters, string email)
    {
        if(email != null && email != "")
        {
            var list = entities.Where(x => x.Email.Contains(email)).Include(p => p.IdNavigation);
            return PagedList<ParkingZoneOwner>.ToPagedList(list, parameters.PageNumber,
            parameters.PageSize);
        }
        else
        {
            var list = entities.Include(x => x.IdNavigation);
            return PagedList<ParkingZoneOwner>.ToPagedList(list, parameters.PageNumber,
            parameters.PageSize);
        }

        
    }
}