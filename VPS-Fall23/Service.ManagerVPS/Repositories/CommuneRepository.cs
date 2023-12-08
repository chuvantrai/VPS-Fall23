using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Service.ManagerVPS.Constants.Enums;
using Service.ManagerVPS.DTO.Exceptions;
using Service.ManagerVPS.DTO.Input;
using Service.ManagerVPS.Extensions.StaticLogic;
using Service.ManagerVPS.Models;
using Service.ManagerVPS.Repositories.Interfaces;

namespace Service.ManagerVPS.Repositories
{
    public class CommuneRepository : VpsRepository<Commune>, ICommuneRepository
    {
        public CommuneRepository(FALL23_SWP490_G14Context fALL23_SWP490_G14Context)
            : base(fALL23_SWP490_G14Context)
        {
        }

        public Task<IEnumerable<Commune>> GetByDistrict(Guid districtId)
        {
            return Task.FromResult(this.entities.Where(c => c.DistrictId == districtId).AsEnumerable());
        }

        public async Task<Tuple<IEnumerable<Commune>, int>> GetListCommune(GetAddressListParkingZoneRequest request)
        {
            request.PageNumber = request.PageNumber == 0 ? 1 : request.PageNumber;
            Expression<Func<Commune, bool>> whereExpression = x => true;
            Expression<Func<Commune, bool>> expr1 = x => true;

            if (request.CityFilter is not null)
            {
                expr1 = expr1.And(x => Equals(x.District.City.Id, request.CityFilter));
                whereExpression = whereExpression.And(expr1);
            }

            if (request.DistrictFilter is not null)
            {
                expr1 = expr1.And(x => Equals(x.District.Id, request.DistrictFilter));
                whereExpression = whereExpression.And(expr1);
            }

            if (!string.IsNullOrEmpty(request.TextAddress))
            {
                expr1 = expr1.And(x => x.Name.ToLower().Contains(request.TextAddress.ToLower()) ||
                                       x.District.Name.ToLower().Contains(request.TextAddress.ToLower()) ||
                                       x.District.City.Name.ToLower().Contains(request.TextAddress.ToLower())
                );
                whereExpression = whereExpression.And(expr1);
            }

            var data = await context.Communes
                .Include(x => x.District)
                .ThenInclude(x => x.City)
                .Where(whereExpression)
                .OrderByDescending(x => x.ModifiedAt)
                .Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize)
                .ToListAsync();
            var totalCommunes = await context.Communes.Where(whereExpression).CountAsync();
            return new Tuple<IEnumerable<Commune>, int>(data, totalCommunes);
        }

        public async Task<bool> UpdateIsBlockCity(bool isBlock, Guid cityId)
        {
            var city = await context.Cities.FirstOrDefaultAsync(x => x.Id.Equals(cityId));
            if (city == null)
            {
                throw new ClientException(3);
            }

            city.IsBlock = isBlock;
            city.ModifiedAt = DateTime.Now;
            await context.SaveChangesAsync();
            return isBlock;
        }
        
        public async Task<bool> UpdateIsBlockDistrict(bool isBlock, Guid districtId)
        {
            var district = await context.Districts.FirstOrDefaultAsync(x => x.Id.Equals(districtId));
            if (district == null)
            {
                throw new ClientException(3);
            }

            district.IsBlock = isBlock;
            district.ModifiedAt = DateTime.Now;
            await context.SaveChangesAsync();
            return isBlock;
        }
        public async Task<bool> UpdateIsBlockCommune(bool isBlock, Guid communeId)
        {
            var communes = await context.Communes.FirstOrDefaultAsync(x => x.Id.Equals(communeId));
            if (communes == null)
            {
                throw new ClientException(3);
            }

            communes.IsBlock = isBlock;
            communes.ModifiedAt = DateTime.Now;
            await context.SaveChangesAsync();
            return isBlock;
        }

        public async Task CreateCommune(CreateAddressRequest request, Guid userId)
        {
            var commune = new Commune()
            {
                Id = Guid.NewGuid(),
                Code = request.Code,
                DistrictId = (Guid)request.District!,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now,
                CreatedBy = userId,
                IsBlock = false,
                Name = request.Name
            };
            await context.Communes.AddAsync(commune);
            await context.SaveChangesAsync();
        }

        public async Task CreateDistrict(CreateAddressRequest request, Guid userId)
        {
            var district = new District()
            {
                Id = Guid.NewGuid(),
                Code = request.Code,
                CityId = (Guid)request.City!,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now,
                CreatedBy = userId,
                Name = request.Name,
                IsBlock = false
            };
            await context.Districts.AddAsync(district);
            await context.SaveChangesAsync();
        }

        public async Task CreateCity(CreateAddressRequest request, Guid userId)
        {
            var city = new City()
            {
                Id = Guid.NewGuid(),
                Code = request.Code,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now,
                CreatedBy = userId,
                Name = request.Name,
                IsBlock = false
            };
            await context.Cities.AddAsync(city);
            await context.SaveChangesAsync();
        }

        public async Task<int> CheckValidate(CreateAddressRequest request)
        {
            switch (request.Type)
            {
                case AddressTypeEnum.CITY:
                    if (!request.Name.Contains("Tỉnh") && !request.Name.Contains("Thành phố"))
                        return 5018; //Tên phải chứa Tỉnh/Thành phố
                    var checkCityCode = await context.Cities
                        .FirstOrDefaultAsync(x => x.Code.Equals(request.Code));
                    var checkCityName = await context.Cities
                        .FirstOrDefaultAsync(x => x.Name.ToLower().Equals(request.Name.ToLower()));
                    if (checkCityCode != null) return 5015; // code địa chỉ này đã tồi tại
                    if (checkCityName != null) return 5016; // Tên địa chỉ này đã tồi tại
                    break;
                case AddressTypeEnum.DISTRICT:
                    var checkDistrictsCode = await context.Districts
                        .FirstOrDefaultAsync(x => x.Code.Equals(request.Code));
                    var checkDistrictsName = await context.Districts
                        .FirstOrDefaultAsync(x => x.Name.ToLower().Equals(request.Name.ToLower()));
                    if (checkDistrictsCode != null) return 5015; // code địa chỉ này đã tồi tại
                    if (checkDistrictsName != null) return 5016; // Tên địa chỉ này đã tồi tại
                    break;
                case AddressTypeEnum.COMMUNE:
                    var checkCommuneCode = await context.Communes
                        .FirstOrDefaultAsync(x => x.Code.Equals(request.Code));
                    var checkCommuneName = await context.Communes
                        .FirstOrDefaultAsync(x => x.Name.ToLower().Equals(request.Name.ToLower()));
                    if (checkCommuneCode != null) return 5015; // code địa chỉ này đã tồi tại
                    if (checkCommuneName != null) return 5016; // Tên địa chỉ này đã tồi tại
                    break;
                default:
                    return -1;
            }

            return -1;
        }
    }
}