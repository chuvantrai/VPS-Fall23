using Microsoft.EntityFrameworkCore;
using Service.ManagerVPS.DTO.Input;
using Service.ManagerVPS.DTO.OtherModels;
using Service.ManagerVPS.DTO.Output;
using Service.ManagerVPS.Models;
using Service.ManagerVPS.Repositories.Interfaces;

namespace Service.ManagerVPS.Repositories;

public class FeedBackRepository : VpsRepository<Feedback>, IFeedBackRepository
{
    public FeedBackRepository(FALL23_SWP490_G14Context context) : base(context)
    {
    }

    public async Task<int> CreateFeedBack(CreateFeedBackParkingZoneRequest request,
        ParkingZone parkingZone)
    {
        if (await context.Feedbacks.FirstOrDefaultAsync(x => x.ParkingZoneId.Equals(parkingZone.Id)
                                                             && request.Email == x.Email) != null)
        {
            return 5010;
        }

        var feedBack = new Feedback()
        {
            Id = Guid.NewGuid(),
            ParkingZoneId = parkingZone.Id,
            Content = request.Content ?? string.Empty,
            Rate = request.Rate,
            CreatedAt = DateTime.Now,
            Email = request.Email
        };
        context.Feedbacks.Add(feedBack);
        await context.SaveChangesAsync();
        feedBack.ParkingZone = parkingZone;
        return 200;
    }

    public PagedList<Feedback> GetListFeedbackForOwner(Guid ownerId,
        QueryStringParameters parameters)
    {
        var lstFeedback = entities
            .AsNoTracking()
            .Include(x => x.InverseParent)
            .Include(x => x.ParkingZone)
            .Where(x => x.ParkingZone.OwnerId.Equals(ownerId) && x.ParentId == null);

        return PagedList<Feedback>.ToPagedList(lstFeedback, parameters.PageNumber,
            parameters.PageSize);
    }
}