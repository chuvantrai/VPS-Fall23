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
    
    public async Task<Feedback?> CreateFeedBack(CreateFeedBackParkingZoneRequest request)
    {
        var parkingZone = await context.ParkingZones
            .FirstOrDefaultAsync(x => x.Id.Equals(request.ParkingZoneId) && x.IsApprove == true);
        if (parkingZone == null) return null;
        var feedBack = new Feedback()
        {
            Id = Guid.NewGuid(),
            ParkingZoneId = parkingZone.Id,
            Content = request.Content??string.Empty,
            Rate = request.Rate,
            CreatedAt = DateTime.Now
        };
        context.Feedbacks.Add(feedBack);
        await context.SaveChangesAsync();
        feedBack.ParkingZone = parkingZone;
        return feedBack;
    }
}