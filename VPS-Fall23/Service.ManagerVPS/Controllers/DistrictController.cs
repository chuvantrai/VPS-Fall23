using Microsoft.AspNetCore.Mvc;
using Service.ManagerVPS.Controllers.Base;
using Service.ManagerVPS.Models;
using Service.ManagerVPS.Repositories.Interfaces;

namespace Service.ManagerVPS.Controllers
{
    public class DistrictController : VpsCRUDController<District>
    {
        public DistrictController(IDistrictRepository districtRepository) : base(districtRepository)
        {
        }

        [HttpGet("GetByCity/{cityId}")]
        public async Task<IEnumerable<District>> GetDistrictByCityId(Guid cityId)
        {
            return await ((IDistrictRepository)this.vpsRepository).GetByCity(cityId);
        }
    }
}
