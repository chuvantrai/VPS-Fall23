using Microsoft.AspNetCore.Mvc;
using Service.ManagerVPS.Controllers.Base;
using Service.ManagerVPS.Models;
using Service.ManagerVPS.Repositories.Interfaces;

namespace Service.ManagerVPS.Controllers
{
    public class CityController : VpsCRUDController<City>
    {
        public CityController(ICityRepository cityRepository) : base(cityRepository) { }
        [HttpGet]
        public override IEnumerable<City> GetList()
        {
            return vpsRepository.Entities.Where(c => (c.IsBlock ?? true) == false).OrderBy(c => c.Name);
        }
    }
}
