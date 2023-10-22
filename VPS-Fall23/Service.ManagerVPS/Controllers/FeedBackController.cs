using Microsoft.AspNetCore.Mvc;
using Service.ManagerVPS.Controllers.Base;
using Service.ManagerVPS.DTO.Exceptions;
using Service.ManagerVPS.DTO.Input;
using Service.ManagerVPS.Models;
using Service.ManagerVPS.Repositories.Interfaces;

namespace Service.ManagerVPS.Controllers;

public class FeedBackController : VpsController<Feedback>
{
    public FeedBackController(IFeedBackRepository feedBackRepository) : base(feedBackRepository)
    {
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateFeedBackParkingZone([FromForm] CreateFeedBackParkingZoneRequest request)
    {
        var feedBack = await ((IFeedBackRepository)vpsRepository).CreateFeedBack(request);
        if (feedBack is null) throw new ClientException();
        return Ok(new
        {
            idParkingZone = feedBack.ParkingZoneId
        });
    }
}