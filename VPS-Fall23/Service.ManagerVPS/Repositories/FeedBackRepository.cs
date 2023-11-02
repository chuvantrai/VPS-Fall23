using Microsoft.EntityFrameworkCore;
using Service.ManagerVPS.DTO.Input;
using Service.ManagerVPS.Models;
using Service.ManagerVPS.Repositories.Interfaces;

namespace Service.ManagerVPS.Repositories;

public class FeedBackRepository : VpsRepository<Feedback>, IFeedBackRepository
{
    public FeedBackRepository(FALL23_SWP490_G14Context context) : base(context)
    {
    }

    public async Task<dynamic> CreateFeedBack(CreateFeedBackParkingZoneRequest request, ParkingZone parkingZone)
    {
        if (await context.Feedbacks.FirstOrDefaultAsync(x => x.ParkingZoneId.Equals(parkingZone.Id)
                                                             && request.Email == x.Email) != null)
        {
            return new
            {
                Id = 5010
            };
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
        return new
        {
            FeedBack = feedBack,
            Id = 200
        };
    }
}