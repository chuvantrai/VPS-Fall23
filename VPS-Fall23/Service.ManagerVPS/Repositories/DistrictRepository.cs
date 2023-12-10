using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Service.ManagerVPS.DTO.Input;
using Service.ManagerVPS.Extensions.StaticLogic;
using Service.ManagerVPS.Models;
using Service.ManagerVPS.Repositories.Interfaces;

namespace Service.ManagerVPS.Repositories
{
    public class DistrictRepository : VpsRepository<District>, IDistrictRepository
    {
        public DistrictRepository(FALL23_SWP490_G14Context fALL23_SWP490_G14Context)
            : base(fALL23_SWP490_G14Context)
        {
        }

        public Task<IEnumerable<District>> GetByCity(Guid cityId)
        {
            return Task.FromResult(entities.Where(d => d.CityId == cityId && (d.IsBlock ?? true) == false).OrderBy(d => d.Name).AsEnumerable());
        }

        public async Task<Tuple<IEnumerable<District>, int>> GetListDistrict(GetAddressListParkingZoneRequest request)
        {
            request.PageNumber = request.PageNumber == 0 ? 1 : request.PageNumber;
            Expression<Func<District, bool>> whereExpression = x => true;
            Expression<Func<District, bool>> expr1 = x => true;

            if (request.CityFilter is not null)
            {
                expr1 = expr1.And(x => Equals(x.City.Id, request.CityFilter));
                whereExpression = whereExpression.And(expr1);
            }

            if (request.DistrictFilter is not null)
            {
                expr1 = expr1.And(x => Equals(x.Id, request.DistrictFilter));
                whereExpression = whereExpression.And(expr1);
            }

            if (!string.IsNullOrEmpty(request.TextAddress))
            {
                expr1 = expr1.And(x => x.Name.ToLower().Contains(request.TextAddress.ToLower()) ||
                                       x.City.Name.ToLower().Contains(request.TextAddress.ToLower())
                );
                whereExpression = whereExpression.And(expr1);
            }

            var data = await context.Districts
                .Include(x => x.City)
                .Where(whereExpression)
                .OrderByDescending(x => x.ModifiedAt)
                .Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize)
                .ToListAsync();
            var totalCommunes = await context.Districts.Where(whereExpression).CountAsync();
            return new Tuple<IEnumerable<District>, int>(data, totalCommunes);
        }
    }
}