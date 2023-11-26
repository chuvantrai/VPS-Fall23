using Service.ManagerVPS.DTO.Input;
using Service.ManagerVPS.Models;

namespace Service.ManagerVPS.Repositories.Interfaces
{
    public interface ICommuneRepository : IVpsRepository<Commune>
    {
        Task<IEnumerable<Commune>> GetByDistrict(Guid districtId);
        
        Task<Tuple<IEnumerable<Commune>, int>> GetListAddress(GetAddressListParkingZoneRequest request);
        
        Task<bool> UpdateIsBlockCommune(bool isBlock,Guid communeId);
        
        Task CreateCommune(CreateAddressRequest request, Guid userId);
        Task CreateDistrict(CreateAddressRequest request, Guid userId);
        Task CreateCity(CreateAddressRequest request, Guid userId);
        Task<int> CheckValidate(CreateAddressRequest request);
    }
}
