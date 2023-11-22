using Service.ManagerVPS.DTO.OtherModels;
using Service.ManagerVPS.Models;

namespace Service.ManagerVPS.Repositories.Interfaces;

public interface IPromoCodeRepository : IVpsRepository<PromoCode>
{
    PagedList<PromoCode> GetListPromoCodeByOwnerId(Guid ownerId, QueryStringParameters parameters);

    PromoCode? GetPromoCodeDetailById(Guid id);

    PromoCode? GetPromoCodeById(Guid id);
}