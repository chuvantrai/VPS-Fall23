using Service.ManagerVPS.Models;
using Service.ManagerVPS.Repositories.Interfaces;

namespace Service.ManagerVPS.Repositories
{
    public class DistrictRepository : VpsRepository<District>, IDistrictRepository
    {
        public DistrictRepository(FALL23_SWP490_G14Context fALL23_SWP490_G14Context)
          : base(fALL23_SWP490_G14Context)
        {

        }

        public Task<IEnumerable<District>> GetByCity(Guid cityId)
        {
            return Task.FromResult(this.entities.Where(d => d.CityId == cityId).AsEnumerable());
        }
    }
}
