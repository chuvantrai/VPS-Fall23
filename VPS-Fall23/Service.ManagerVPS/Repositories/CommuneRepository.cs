using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
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

        public async Task<Tuple<IEnumerable<Commune>, int>> GetListDistrict(GetAddressListParkingZoneRequest request)
        {
            request.PageNumber = request.PageNumber == 0 ? 1 : request.PageNumber;
            Expression<Func<Commune, bool>> whereExpression = x => true;
            Expression<Func<Commune, bool>> expr1 = x => true;
            Expression<Func<Commune, bool>> expr2 = x => false;

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
                expr1 = expr1.And(x => x.Name.Contains(request.TextAddress) ||
                                       x.District.Name.Contains(request.TextAddress) ||
                                       x.District.City.Name.Contains(request.TextAddress)
                );
                whereExpression = whereExpression.And(expr1);
            }
            
            var data = await context.Communes
                .Include(x => x.District)
                .ThenInclude(x => x.City)
                .Where(whereExpression)
                .OrderByDescending(x => x.SubId)
                .Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize)
                .ToListAsync();
            var totalCommunes = await context.Communes.Where(whereExpression).CountAsync();
            return new Tuple<IEnumerable<Commune>, int>(data, totalCommunes);
        }
    }
}