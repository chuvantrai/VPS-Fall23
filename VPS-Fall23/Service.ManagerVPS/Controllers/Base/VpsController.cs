using Microsoft.AspNetCore.Mvc;
using Service.ManagerVPS.Repositories.Interfaces;

namespace Service.ManagerVPS.Controllers.Base
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class VpsController<T> : Controller
        where T : class
    {
        protected readonly IVpsRepository<T> vpsRepository;

        public VpsController(IVpsRepository<T> vpsRepository)
        {
            this.vpsRepository = vpsRepository;
        }
        [ApiExplorerSettings(IgnoreApi = true)] 
        public string GetIpAddress()
        {
            string ipAddress = string.Empty;
            try
            {
                ipAddress = HttpContext.GetServerVariable("HTTP_X_FORWARDED_FOR");

                if (string.IsNullOrEmpty(ipAddress) || (ipAddress.ToLower() == "unknown") || ipAddress.Length > 45)
                    ipAddress = HttpContext.GetServerVariable("REMOTE_ADDR");
            }
            catch
            {
            }

            return ipAddress ?? "::1";
        }
    }
}