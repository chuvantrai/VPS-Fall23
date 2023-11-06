using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Service.ManagerVPS.Controllers.Base;
using Service.ManagerVPS.DTO.AppSetting;
using Service.ManagerVPS.DTO.OtherModels;
using Service.ManagerVPS.DTO.Ouput;
using Service.ManagerVPS.Extensions.StaticLogic;
using Service.ManagerVPS.Models;
using Service.ManagerVPS.Repositories.Interfaces;

namespace Service.ManagerVPS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParkingZoneOwnerController : VpsController<ParkingZoneOwner>
    {
        private readonly IConfiguration _config;
        private readonly FileManagementConfig _fileManagementConfig;
        private readonly IContractRepository _contractRepository;

        public ParkingZoneOwnerController(IParkingZoneOwnerRepository parkingZoneRepository,
            IConfiguration config, IOptions<FileManagementConfig> options,
            IContractRepository contractRepository)
            : base(parkingZoneRepository)
        {
            _config = config;
            _fileManagementConfig = options.Value;
            _contractRepository = contractRepository;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll([FromQuery] QueryStringParameters parameters)
        {
            try
            {
                var accessToken = Request.Cookies["ACCESS_TOKEN"]!;
                var userToken = JwtTokenExtension.ReadToken(accessToken)!;
                var list = ((IParkingZoneOwnerRepository)vpsRepository).GetAllOwner(parameters);

                //if (userToken.RoleId == 3 || userToken.RoleId == 2) return NotFound();

                var metadata = new
                {
                    list.TotalCount,
                    list.PageSize,
                    list.CurrentPage,
                    list.TotalPages,
                    list.HasNext,
                    list.HasPrev,
                    Data = list
                };
                return Ok(metadata);
            }
            catch (Exception ex)
            {
                return NotFound(ex);
            }
        }

        [HttpGet("GetByEmail")]
        public IActionResult GetByEmail([FromQuery] QueryStringParameters parameters, string email)
        {
            try
            {
                var accessToken = Request.Cookies["ACCESS_TOKEN"]!;
                var userToken = JwtTokenExtension.ReadToken(accessToken)!;
                var list = ((IParkingZoneOwnerRepository)vpsRepository).GetOwnerByEmail(parameters, email);
                List<ParkingZoneOwner> res = new List<ParkingZoneOwner>();

                if (userToken.RoleId == 3 || userToken.RoleId == 2) return NotFound();

                foreach (ParkingZoneOwner item in list)
                {
                    res.Add(new ParkingZoneOwner
                    {
                        Id = item.Id,
                        ModifiedAt = item.ModifiedAt,
                        CreatedAt = item.CreatedAt,
                        Phone = item.Phone,
                        Email = item.Email,
                        Dob = item.Dob,
                    });
                }
                var metadata = new
                {
                    list.TotalCount,
                    list.PageSize,
                    list.CurrentPage,
                    list.TotalPages,
                    list.HasNext,
                    list.HasPrev,
                    Data = res
                };
                return Ok(metadata);
            }
            catch (Exception ex)
            {
                return NotFound(ex);
            }
        }
    }
}
