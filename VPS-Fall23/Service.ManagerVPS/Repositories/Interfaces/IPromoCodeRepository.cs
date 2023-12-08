using Service.ManagerVPS.Models;

namespace Service.ManagerVPS.Repositories.Interfaces;

public interface IPromoCodeRepository : IVpsRepository<PromoCode>
{
    Task<PromoCode?> GetByCode(string promoCode, Guid parkingZoneId);
}