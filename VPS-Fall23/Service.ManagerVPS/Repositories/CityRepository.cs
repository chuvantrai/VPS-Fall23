using Microsoft.EntityFrameworkCore;
using Service.ManagerVPS.DTO.Input;
using Service.ManagerVPS.Models;
using Service.ManagerVPS.Repositories.Interfaces;

namespace Service.ManagerVPS.Repositories
{
    public class CityRepository : VpsRepository<City>, ICityRepository
    {
        public CityRepository(FALL23_SWP490_G14Context fall23Swp490G14Context)
            : base(fall23Swp490G14Context)
        {
        }

        public async Task<Tuple<IEnumerable<City>, int>> GetListCity(GetAddressListParkingZoneRequest request)
        {
            request.PageNumber = request.PageNumber == 0 ? 1 : request.PageNumber;
            var data = await context.Cities
                .Where(x => x.Name.ToLower().Contains(request.TextAddress.ToLower()))
                .OrderByDescending(x => x.ModifiedAt)
                .Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize)
                .ToListAsync();
            var totalCity = await context.Cities
                .Where(x => x.Name.ToLower().Contains(request.TextAddress.ToLower()))
                .CountAsync();
            return new Tuple<IEnumerable<City>, int>(data, totalCity);
        }
    }
}