using Microsoft.AspNetCore.Mvc;
using Service.ManagerVPS.Controllers.Base;
using Service.ManagerVPS.DTO.Exceptions;
using Service.ManagerVPS.DTO.Input;
using Service.ManagerVPS.Models;
using Service.ManagerVPS.Repositories.Interfaces;

namespace Service.ManagerVPS.Controllers;

public class FeedBackController : VpsController<Feedback>
{
    private readonly IParkingTransactionRepository _parkingTransactionRepository;

    public FeedBackController(IFeedBackRepository feedBackRepository,
        IParkingTransactionRepository parkingTransactionRepository)
        : base(feedBackRepository)
    {
        _parkingTransactionRepository = parkingTransactionRepository;
    }

    [HttpPost]
    public async Task<IActionResult> CreateFeedBackParkingZone(CreateFeedBackParkingZoneRequest request)
    {
        var parkingTransaction =
            await _parkingTransactionRepository.GetParkingTransactionByIdEmail(request.ParkingZoneId, request.Email);
        if (parkingTransaction.Id != 200) throw new ClientException(parkingTransaction.id);

        var feedBackResult = await ((IFeedBackRepository)vpsRepository)
            .CreateFeedBack(request, (ParkingZone)parkingTransaction.ParkingTransaction.ParkingZone);
        
        if (feedBackResult.Id != 200) throw new ClientException(feedBackResult.Id);
        return Ok(new
        {
            FeedBackResult = feedBackResult.FeedBack
        });
    }
}