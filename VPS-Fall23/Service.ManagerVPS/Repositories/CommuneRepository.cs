using Microsoft.EntityFrameworkCore;
using Service.ManagerVPS.DTO.Input;
using Service.ManagerVPS.Models;
using Service.ManagerVPS.Repositories.Interfaces;

namespace Service.ManagerVPS.Repositories
{
    public class CommuneRepository : VpsRepository<Commune>, ICommuneRepository
    {
        public CommuneRepository(FALL23_SWP490_G14Context fALL23_SWP490_G14Context)
            : base(fALL23_SWP490_G14Context)
        {
        }

        public Task<IEnumerable<Commune>> GetByDistrict(Guid districtId)
        {
            return Task.FromResult(this.entities.Where(c => c.DistrictId == districtId).AsEnumerable());
        }

        public async Task<Tuple<IEnumerable<Commune>, int>> GetListDistrict(GetAddressListParkingZoneRequest request)
        {
            request.PageNumber = request.PageNumber == 0 ? 1 : request.PageNumber;
            var data = await context.Communes
                .Include(x => x.District)
                .ThenInclude(x => x.City)
                .OrderByDescending(x => x.SubId)
                .Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize)
                .ToListAsync();
            var totalCommunes = await context.Communes.CountAsync();
            var totalPages = (int)Math.Ceiling(totalCommunes / (double)request.PageSize);
            return new Tuple<IEnumerable<Commune>, int>(data, totalPages);
        }
    }
}