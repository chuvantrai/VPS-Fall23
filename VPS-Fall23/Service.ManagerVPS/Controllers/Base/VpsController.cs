using Microsoft.AspNetCore.Mvc;
using Service.ManagerVPS.Attributes;
using Service.ManagerVPS.Repositories.Interfaces;

namespace Service.ManagerVPS.Controllers.Base
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [VpsActionFilter]
    public class VpsController<T> : Controller
        where T : class
    {
        protected readonly IVpsRepository<T> vpsRepository;

        public VpsController(IVpsRepository<T> vpsRepository)
        {
            this.vpsRepository = vpsRepository;
        }
    }
}