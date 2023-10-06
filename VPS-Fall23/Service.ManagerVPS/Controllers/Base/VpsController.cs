using Microsoft.AspNetCore.Mvc;
using Service.ManagerVPS.Attributes;

namespace Service.ManagerVPS.Controllers.Base
{
    [ApiController]
    [Route("api/[controller]")]
    [VpsActionFilter]
    public class VpsController : Controller    
    {
    }
}
