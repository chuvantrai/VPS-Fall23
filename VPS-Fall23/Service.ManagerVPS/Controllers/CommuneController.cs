using Microsoft.AspNetCore.Mvc;
using Service.ManagerVPS.Constants.Enums;
using Service.ManagerVPS.Controllers.Base;
using Service.ManagerVPS.DTO.Exceptions;
using Service.ManagerVPS.DTO.Input;
using Service.ManagerVPS.Extensions.StaticLogic;
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
                .GetListAddress(request);

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
        [HttpPut("UpdateIsBlockCommune")]
        public async Task<IActionResult> UpdateIsBlockCommune(UpdateIsBlockCommuneRequest request)
        {
            var isBlockAfterUpdate = await ((ICommuneRepository)vpsRepository)
                .UpdateIsBlockCommune(request.IsBlock, request.CommuneId);
            return Ok(isBlockAfterUpdate);
        }
        
        [HttpPost("CreateAddress")]
        public async Task<IActionResult> CreateAddress(CreateAddressRequest request)
        {
            var accessToken = Request.Cookies["ACCESS_TOKEN"]!;
            var userToken = JwtTokenExtension.ReadToken(accessToken)!;
            var checkValidate = await ((ICommuneRepository)vpsRepository).CheckValidate(request);
            if (checkValidate != -1)
            {
                throw new ClientException(checkValidate);
            }
            switch (request.Type)
            {
                case AddressTypeEnum.CITY:
                    await ((ICommuneRepository)vpsRepository)
                        .CreateCity(request, Guid.Parse(userToken.UserId));
                    break;
                case AddressTypeEnum.DISTRICT:
                    await ((ICommuneRepository)vpsRepository)
                        .CreateDistrict(request, Guid.Parse(userToken.UserId));
                    break;
                case AddressTypeEnum.COMMUNE:
                    await ((ICommuneRepository)vpsRepository)
                        .CreateCommune(request, Guid.Parse(userToken.UserId));
                    break;
                default:
                    throw new ClientException(5017);// Type tạo địa chỉ không tồn tại
            }
            
            return Ok();
        }
    }
}