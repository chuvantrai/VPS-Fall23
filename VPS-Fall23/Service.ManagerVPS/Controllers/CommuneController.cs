using Microsoft.AspNetCore.Mvc;
using Service.ManagerVPS.Controllers.Base;
using Service.ManagerVPS.DTO.Input;
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

        [HttpGet("GetAddressListParkingZone")]
        public async Task<IActionResult> GetAddressListParkingZone([FromQuery] GetAddressListParkingZoneRequest request)
        {
            var dataGetListDistrict = await ((ICommuneRepository)vpsRepository)
                .GetListDistrict(request);

            return Ok(new
            {
                ListAddress = dataGetListDistrict.Item1.Select(x => new
                {
                    
                    CityCode = x.District.City.Code,
                    CityId = x.District.City.Id,
                    CityName = x.District.City.Name,
                    DistrictCode = x.District.Code,
                    DistrictId = x.District.Id,
                    DistrictName = x.District.Name,
                    CommuneCode = x.Code,
                    CommuneId = x.Id,
                    CommuneName = x.Name,
                    x.CreatedAt,
                    x.ModifiedAt,
                    IsBlock = x.IsBlock ?? false
                }).AsEnumerable(),
                TotalPages = dataGetListDistrict.Item2
            });
        }
    }
}