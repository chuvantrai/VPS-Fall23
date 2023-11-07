using Microsoft.AspNetCore.Mvc;
using Service.ManagerVPS.Constants.Enums;
using Service.ManagerVPS.Constants.Notifications;
using Service.ManagerVPS.Controllers.Base;
using Service.ManagerVPS.DTO.Exceptions;
using Service.ManagerVPS.DTO.Input;
using Service.ManagerVPS.DTO.OtherModels;
using Service.ManagerVPS.FilterPermissions;
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
    public async Task<IActionResult> CreateFeedBackParkingZone(
        CreateFeedBackParkingZoneRequest request)
    {
        var parkingTransaction =
            await _parkingTransactionRepository.GetParkingTransactionByIdEmail(
                request.ParkingZoneId, request.Email);
        if (parkingTransaction == null) throw new ClientException(5008);

        var feedBackResult =
            await ((IFeedBackRepository)vpsRepository).CreateFeedBack(request,
                parkingTransaction.ParkingZone);
        if (feedBackResult != 200) throw new ClientException(feedBackResult);
        return Ok();
    }

    [HttpGet("{parkingZoneId}")]
    public QueryResponseModel<Feedback> GetFeedbacksByParkingZone(Guid parkingZoneId, int page = 1,
        int pageSize = 10)
    {
        var result = new QueryResponseModel<Feedback>(page, pageSize,
            vpsRepository.Entities.Where(p => p.ParkingZoneId == parkingZoneId)
                .OrderByDescending(p => p.Rate));
        return result;
    }

    [HttpGet]
    [FilterPermission(Action = ActionFilterEnum.GetFeedbackForOwner)]
    public IActionResult GetFeedbackForOwner([FromQuery] Guid ownerId,
        [FromQuery] QueryStringParameters parameters)
    {
        var lstFeedback =
            ((IFeedBackRepository)vpsRepository).GetListFeedbackForOwner(ownerId, parameters);

        var result = lstFeedback
            .Select((x, index) => new
            {
                Key = index + 1,
                x.SubId,
                x.Id,
                x.ParkingZoneId,
                ParkingZoneName = x.ParkingZone.Name,
                x.Email,
                CreatedAt = $"{x.CreatedAt:dd-MM-yyyy}",
                x.Rate,
                x.Content,
                Replies = x.InverseParent
                    .OrderBy(y => y.SubId)
                    .Select((y, idx) => new
                    {
                        ChildKey = idx + 1,
                        y.SubId,
                        y.Id,
                        y.Content
                    })
            }).ToList();

        var metadata = new
        {
            lstFeedback.TotalCount,
            lstFeedback.PageSize,
            lstFeedback.CurrentPage,
            lstFeedback.TotalPages,
            lstFeedback.HasNext,
            lstFeedback.HasPrev,
            Data = result
        };
        return Ok(metadata);
    }

    [HttpPost]
    [FilterPermission(Action = ActionFilterEnum.AddReplyToFeedback)]
    public async Task<IActionResult> AddReplyToFeedback([FromBody] AddReplyToFeedbackInput input)
    {
        var feedback =
            ((IFeedBackRepository)vpsRepository).GetFeedbackById((Guid)input.FeedbackId!);
        if (feedback is null)
        {
            throw new ServerException(2);
        }

        var reply = new Feedback
        {
            Id = Guid.NewGuid(),
            ParkingZoneId = feedback.ParkingZoneId,
            Content = input.Content,
            CreatedAt = DateTime.Now,
            ParentId = feedback.Id
        };
        await ((IFeedBackRepository)vpsRepository).Create(reply);
        await ((IFeedBackRepository)vpsRepository).SaveChange();
        return Ok(ResponseNotification.ADD_SUCCESS);
    }

    [HttpGet]
    [FilterPermission(Action = ActionFilterEnum.FilterFeedback)]
    public IActionResult FilterFeedback([FromQuery] Guid ownerId,
        [FromQuery] QueryStringParameters parameters, [FromQuery] string? parkingZoneId,
        [FromQuery] string? rate)
    {
        var lstFeedback =
            ((IFeedBackRepository)vpsRepository).FilterFeedbackForOwner(ownerId, parameters,
                parkingZoneId, rate);

        var result = lstFeedback
            .Select((x, index) => new
            {
                Key = index + 1,
                x.SubId,
                x.Id,
                x.ParkingZoneId,
                ParkingZoneName = x.ParkingZone.Name,
                x.Email,
                CreatedAt = $"{x.CreatedAt:dd-MM-yyyy}",
                x.Rate,
                x.Content,
                Replies = x.InverseParent
                    .OrderBy(y => y.SubId)
                    .Select((y, idx) => new
                    {
                        ChildKey = idx + 1,
                        y.SubId,
                        y.Id,
                        y.Content
                    })
            }).ToList();

        var metadata = new
        {
            lstFeedback.TotalCount,
            lstFeedback.PageSize,
            lstFeedback.CurrentPage,
            lstFeedback.TotalPages,
            lstFeedback.HasNext,
            lstFeedback.HasPrev,
            Data = result
        };
        return Ok(metadata);
    }
}