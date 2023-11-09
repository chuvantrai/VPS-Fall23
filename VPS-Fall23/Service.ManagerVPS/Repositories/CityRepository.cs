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
    }
}
