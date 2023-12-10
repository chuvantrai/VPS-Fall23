using Service.ManagerVPS.DTO.OtherModels;
using Service.ManagerVPS.Models;

namespace Service.ManagerVPS.Repositories.Interfaces;

public interface IPromoCodeInfoRepository : IVpsRepository<PromoCodeInformation>
{
    PagedList<PromoCodeInformation> GetListPromoCodeByOwnerId(Guid ownerId,
        QueryStringParameters parameters);

    PromoCodeInformation? GetPromoCodeInfoDetailById(Guid infoId);
    
    Task UpdateIsSendPromoCode(List<Guid> parkingZoneIds);
}