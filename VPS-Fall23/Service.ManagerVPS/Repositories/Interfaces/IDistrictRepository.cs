using Service.ManagerVPS.Models;

namespace Service.ManagerVPS.Repositories.Interfaces
{
    public interface IDistrictRepository : IVpsRepository<District>
    {
        Task<IEnumerable<District>> GetByCity(Guid cityId);
    }
}
