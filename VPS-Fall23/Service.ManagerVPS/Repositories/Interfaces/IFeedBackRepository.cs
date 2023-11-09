using Service.ManagerVPS.DTO.Input;
using Service.ManagerVPS.DTO.OtherModels;
using Service.ManagerVPS.Models;

namespace Service.ManagerVPS.Repositories.Interfaces;

public interface IFeedBackRepository : IVpsRepository<Feedback>
{
    PagedList<Feedback> GetListFeedbackForOwner(Guid ownerId, QueryStringParameters parameters);

    Feedback? GetFeedbackById(Guid id);

    PagedList<Feedback> FilterFeedbackForOwner(Guid ownerId, QueryStringParameters parameters,
        string parkingZoneId, string rate);

    Task<dynamic> CreateFeedBack(CreateFeedBackParkingZoneRequest request, ParkingZone parkingZone);
}