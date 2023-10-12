using Service.ManagerVPS.Controllers.Base;
using Service.ManagerVPS.Models;
using Service.ManagerVPS.Repositories.Interfaces;

namespace Service.ManagerVPS.Controllers
{
    public class CityController : VpsCRUDController<City>
    {
        public CityController(ICityRepository cityRepository) : base(cityRepository) { }
    }
}
