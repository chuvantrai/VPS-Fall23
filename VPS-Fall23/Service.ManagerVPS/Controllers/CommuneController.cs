using Microsoft.AspNetCore.Mvc;
using Service.ManagerVPS.Controllers.Base;
using Service.ManagerVPS.Models;
using Service.ManagerVPS.Repositories.Interfaces;

namespace Service.ManagerVPS.Controllers
{
    public class CommuneController : VpsCRUDController<Commune>
    {
        public CommuneController(ICommuneRepository communeRepository) : base(communeRepository)
        {
        }

        [HttpGet("GetByDistrict/{districtId}")]
        public async Task<IEnumerable<Commune>> GetCommuneByDistrict(Guid districtId)
        {
            return await ((ICommuneRepository)this.vpsRepository).GetByDistrict(districtId);
        }
    }
}