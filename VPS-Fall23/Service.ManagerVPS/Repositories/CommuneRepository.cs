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
    }
}
