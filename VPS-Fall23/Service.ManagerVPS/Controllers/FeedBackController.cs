using Microsoft.AspNetCore.Mvc;
using Service.ManagerVPS.Controllers.Base;
using Service.ManagerVPS.DTO.Exceptions;
using Service.ManagerVPS.DTO.Input;
using Service.ManagerVPS.Models;
using Service.ManagerVPS.Repositories.Interfaces;

namespace Service.ManagerVPS.Controllers;

public class FeedBackController : VpsController<Feedback>
{
    public readonly IParkingTransactionRepository _parkingTransactionRepository;

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
        if (parkingTransaction == null) throw new ClientException(5008);
        
        var feedBackResult = await ((IFeedBackRepository)vpsRepository).CreateFeedBack(request, parkingTransaction.ParkingZone);
        if (feedBackResult != 200) throw new ClientException(feedBackResult);
        return Ok();
    }
}