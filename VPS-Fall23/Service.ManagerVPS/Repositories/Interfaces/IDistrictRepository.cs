using Service.ManagerVPS.DTO.Input;
using Service.ManagerVPS.Models;

namespace Service.ManagerVPS.Repositories.Interfaces
{
    public interface IDistrictRepository : IVpsRepository<District>
    {
        Task<IEnumerable<District>> GetByCity(Guid cityId);
        
        Task<Tuple<IEnumerable<District>, int>> GetListDistrict(GetAddressListParkingZoneRequest request);
    }
}
