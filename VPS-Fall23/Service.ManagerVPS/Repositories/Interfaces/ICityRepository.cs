using Service.ManagerVPS.DTO.Input;
using Service.ManagerVPS.Models;

namespace Service.ManagerVPS.Repositories.Interfaces
{
    public interface ICityRepository : IVpsRepository<City>
    {
        Task<Tuple<IEnumerable<City>, int>> GetListCity(GetAddressListParkingZoneRequest request);
    }
}
