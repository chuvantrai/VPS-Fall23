using Service.ManagerVPS.DTO.Input;
using Service.ManagerVPS.Models;

namespace Service.ManagerVPS.Repositories.Interfaces
{
    public interface ICommuneRepository : IVpsRepository<Commune>
    {
        Task<IEnumerable<Commune>> GetByDistrict(Guid districtId);
        
        Task<Tuple<IEnumerable<Commune>, int>> GetListDistrict(GetAddressListParkingZoneRequest request);
    }
}
