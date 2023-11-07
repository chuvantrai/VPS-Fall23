using Service.ManagerVPS.DTO.Input;
using Service.ManagerVPS.DTO.OtherModels;
using Service.ManagerVPS.Models;

namespace Service.ManagerVPS.Repositories.Interfaces;

public interface IFeedBackRepository : IVpsRepository<Feedback>
{
    Task<int> CreateFeedBack(CreateFeedBackParkingZoneRequest request, ParkingZone parkingZone);

    PagedList<Feedback> GetListFeedbackForOwner(Guid ownerId, QueryStringParameters parameters);

    Feedback? GetFeedbackById(Guid id);
}