using Service.ManagerVPS.Models;
using Service.ManagerVPS.Repositories.Interfaces;

namespace Service.ManagerVPS.Repositories
{
    public class CityRepository : VpsRepository<City>, ICityRepository
    {
        public CityRepository(FALL23_SWP490_G14Context fALL23_SWP490_G14Context)
          : base(fALL23_SWP490_G14Context)
        {

        }
    }
}
