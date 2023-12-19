using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service.ManagerVPS.Constants.Enums;
using Service.ManagerVPS.Controllers.Base;
using Service.ManagerVPS.DTO.Exceptions;
using Service.ManagerVPS.DTO.Input;
using Service.ManagerVPS.Extensions.StaticLogic;
using Service.ManagerVPS.Models;
using Service.ManagerVPS.Repositories.Interfaces;
using Service.ManagerVPS.FilterPermissions;

namespace Service.ManagerVPS.Controllers
{
    public class CommuneController : VpsCRUDController<Commune>
    {
        private readonly IDistrictRepository _districtRepository;
        private readonly ICityRepository _cityRepository;

        public CommuneController(ICommuneRepository communeRepository,
            IDistrictRepository districtRepository, ICityRepository cityRepository) : base(communeRepository)
        {
            _districtRepository = districtRepository;
            _cityRepository = cityRepository;
        }
        [HttpGet("FindAddressById/{id}")]
        public async Task<IActionResult> FindAddressById(Guid id)
        {
            try
            {
                return Ok(await this.vpsRepository
                    .Entities
                    .Include(c => c.District)
                    .ThenInclude(d => d.City)
                    .FirstOrDefaultAsync(c => c.Id == id)
                    ?? throw new Exception());
            }
            catch (Exception)
            {
                try
                {

                    return Ok(await this._districtRepository
                        .Entities
                        .Include(d => d.City)
                        .FirstOrDefaultAsync(c => c.Id == id)
                          ?? throw new Exception());

                }
                catch (Exception)
                {
                    return Ok(await this._cityRepository.Find(id));
                }
            }

        }
        [HttpGet("GetByDistrict/{districtId}")]
        public async Task<IEnumerable<Commune>> GetCommuneByDistrict(Guid districtId)
        {
            return await ((ICommuneRepository)this.vpsRepository).GetByDistrict(districtId);
        }

        [HttpGet("GetAddressListParkingZone")]
        [FilterPermission(Action = ActionFilterEnum.GetAddressListParkingZone)]
        public async Task<IActionResult> GetAddressListParkingZone([FromQuery] GetAddressListParkingZoneRequest request)
        {
            if (!string.IsNullOrEmpty(request.TextAddress))
            {
                request.TextAddress = request.TextAddress.Trim();
            }
            switch (request.TypeAddress)
            {
                case AddressTypeEnum.COMMUNE:
                    var dataCommune = await ((ICommuneRepository)vpsRepository)
                        .GetListCommune(request);
                    return Ok(new
                    {
                        ListAddress = dataCommune.Item1.Select(x => new
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
                        TotalPages = dataCommune.Item2
                    });
                case AddressTypeEnum.DISTRICT:
                    var dataDistrict = await _districtRepository.GetListDistrict(request);
                    return Ok(new
                    {
                        ListAddress = dataDistrict.Item1.Select(x => new
                        {
                            CityCode = x.City.Code,
                            CityId = x.City.Id,
                            CityName = x.City.Name,
                            DistrictCode = x.Code,
                            DistrictId = x.Id,
                            DistrictName = x.Name,
                            x.CreatedAt,
                            x.ModifiedAt,
                            IsBlock = x.IsBlock ?? false
                        }).AsEnumerable(),
                        TotalPages = dataDistrict.Item2
                    });
                case AddressTypeEnum.CITY:
                    var dataCity = await _cityRepository
                        .GetListCity(request);
                    return Ok(new
                    {
                        ListAddress = dataCity.Item1.Select(x => new
                        {
                            CityCode = x.Code,
                            CityId = x.Id,
                            CityName = x.Name,
                            x.CreatedAt,
                            x.ModifiedAt,
                            IsBlock = x.IsBlock ?? false
                        }).AsEnumerable(),
                        TotalPages = dataCity.Item2
                    });
                default:
                    throw new ClientException(3);
            }
        }

        [HttpPut("UpdateIsBlockAddress")]
        // [FilterPermission(Action = ActionFilterEnum.UpdateIsBlockAddress)]
        public async Task<IActionResult> UpdateIsBlockAddress(UpdateIsBlockAddressRequest request)
        {
            return request.TypeAddress switch
            {
                AddressTypeEnum.COMMUNE => Ok(
                    await ((ICommuneRepository)vpsRepository).UpdateIsBlockCommune(request.IsBlock, request.CommuneId)),
                AddressTypeEnum.DISTRICT => Ok(
                    await ((ICommuneRepository)vpsRepository).UpdateIsBlockDistrict(request.IsBlock,
                        request.CommuneId)),
                AddressTypeEnum.CITY => Ok(
                    await ((ICommuneRepository)vpsRepository).UpdateIsBlockCity(request.IsBlock, request.CommuneId)),
                _ => throw new ClientException(3)
            };
        }

        [HttpPost("CreateAddress")]
        [FilterPermission(Action = ActionFilterEnum.CreateAddress)]
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
                    throw new ClientException(5017); // Type tạo địa chỉ không tồn tại
            }

            return Ok();
        }
    }
}