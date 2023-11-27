using Service.ManagerVPS.DTO.Input;
using Service.ManagerVPS.Models;

namespace Service.ManagerVPS.Repositories.Interfaces
{
    public interface ICommuneRepository : IVpsRepository<Commune>
    {
        Task<IEnumerable<Commune>> GetByDistrict(Guid districtId);
        
        Task<Tuple<IEnumerable<Commune>, int>> GetListCommune(GetAddressListParkingZoneRequest request);
        
        Task<bool> UpdateIsBlockCommune(bool isBlock,Guid communeId);
        Task<bool> UpdateIsBlockDistrict(bool isBlock,Guid communeId);
        Task<bool> UpdateIsBlockCity(bool isBlock,Guid communeId);
        
        Task CreateCommune(CreateAddressRequest request, Guid userId);
        Task CreateDistrict(CreateAddressRequest request, Guid userId);
        Task CreateCity(CreateAddressRequest request, Guid userId);
        Task<int> CheckValidate(CreateAddressRequest request);
    }
}
