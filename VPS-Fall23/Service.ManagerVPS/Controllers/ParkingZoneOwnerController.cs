using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Service.ManagerVPS.Constants.Enums;
using Service.ManagerVPS.Controllers.Base;
using Service.ManagerVPS.DTO.AppSetting;
using Service.ManagerVPS.DTO.OtherModels;
using Service.ManagerVPS.Extensions.StaticLogic;
using Service.ManagerVPS.FilterPermissions;
using Service.ManagerVPS.Models;
using Service.ManagerVPS.Repositories.Interfaces;

namespace Service.ManagerVPS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParkingZoneOwnerController : VpsController<ParkingZoneOwner>
    {
        private readonly IConfiguration _config;
        private readonly FileManagementConfig _fileManagementConfig;
        private readonly IContractRepository _contractRepository;
        readonly IParkingTransactionRepository parkingTransactionRepository;

        public ParkingZoneOwnerController(IParkingZoneOwnerRepository parkingZoneRepository,
            IParkingTransactionRepository parkingTransactionRepository,
            IConfiguration config, IOptions<FileManagementConfig> options,
            IContractRepository contractRepository)
            : base(parkingZoneRepository)
        {
            _config = config;
            _fileManagementConfig = options.Value;
            _contractRepository = contractRepository;
            this.parkingTransactionRepository = parkingTransactionRepository;
        }

        [HttpGet("GetAll")]
        [FilterPermission(Action = ActionFilterEnum.GetAllParkingZoneOwner)]
        public IActionResult GetAll([FromQuery] QueryStringParameters parameters)
        {
            try
            {
                var list = ((IParkingZoneOwnerRepository)vpsRepository).GetAllOwner(parameters);
                var result = list
                    .Select(x => new
                    {
                        x.Id,
                        x.Email,
                        x.Phone,
                        x.Dob,
                        x.CreatedAt,
                        x.ModifiedAt,
                        x.IdNavigation.Username,
                        FullName = x.IdNavigation.FirstName + " " + x.IdNavigation.LastName,
                        x.IdNavigation.Address,
                        x.IdNavigation.IsBlock
                    })
                    .ToList();
                var metadata = new
                {
                    list.TotalCount,
                    list.PageSize,
                    list.CurrentPage,
                    list.TotalPages,
                    list.HasNext,
                    list.HasPrev,
                    Data = result
                };
                return Ok(metadata);
            }
            catch (Exception ex)
            {
                return NotFound(ex);
            }
        }

        [HttpGet("GetByEmail")]
        public IActionResult GetByEmail([FromQuery] QueryStringParameters parameters, string email)
        {
            try
            {
                var accessToken = Request.Cookies["ACCESS_TOKEN"]!;
                var userToken = JwtTokenExtension.ReadToken(accessToken)!;
                var list =
                    ((IParkingZoneOwnerRepository)vpsRepository).GetOwnerByEmail(parameters, email);

                if (userToken.RoleId == 3 || userToken.RoleId == 2) return NotFound();

                var result = list
                   .Select(x => new
                   {
                       x.Id,
                       x.Email,
                       x.Phone,
                       x.Dob,
                       x.CreatedAt,
                       x.ModifiedAt,
                       x.IdNavigation.Username,
                       FullName = x.IdNavigation.FirstName + " " + x.IdNavigation.LastName,
                       x.IdNavigation.Address,
                       x.IdNavigation.IsBlock
                   })
                   .ToList();

                var metadata = new
                {
                    list.TotalCount,
                    list.PageSize,
                    list.CurrentPage,
                    list.TotalPages,
                    list.HasNext,
                    list.HasPrev,
                    Data = result
                };
                return Ok(metadata);
            }
            catch (Exception ex)
            {
                return NotFound(ex);
            }
        }

        [HttpGet("GetBookedOverview")]
        public IActionResult GetBookedOverview([FromQuery] string? parkingZoneName)
        {
            try
            {
                var accessToken = Request.Cookies["ACCESS_TOKEN"]!;
                var userToken = JwtTokenExtension.ReadToken(accessToken)!;

                Guid ownerId = new Guid(userToken.UserId);

                DateTime now = DateTime.Now;
                var list = parkingTransactionRepository.GetBookedSlot(parkingZoneName, ownerId, DateTime.Now);
                using (var context = new FALL23_SWP490_G14Context())
                {
                    var doneCheckInOut = 0;
                    var notCheckIn = 0;
                    var notCheckOut = 0;
                    decimal hourCash = 0;
                    decimal dayCash = 0;
                    decimal weekCash = 0;
                    decimal monthCash = 0;
                    decimal yearCash = 0;
                    foreach (var item in list)
                    {
                        var detail = context.ParkingTransactionDetails.Where(x => x.ParkingTransactionId == item.Id).FirstOrDefault();

                        if (detail.From < DateTime.Now && detail.To < DateTime.Now)
                        {
                            doneCheckInOut++;
                        }

                        if (detail.From > DateTime.Now || detail.To == null)
                        {
                            notCheckIn++;
                        }

                        if (detail.From <= DateTime.Now &&
                            (detail.To > DateTime.Now || detail.To == null))
                        {
                            notCheckOut++;
                        }

                        TimeSpan timeDifference = (item.CheckoutAt - item.CheckinAt).Value;

                        DayOfWeek
                            firstDayOfWeek =
                                DayOfWeek
                                    .Monday; // You can adjust this to your preferred first day of the week

                        DateTime startOfWeek = now.AddDays(-(int)now.DayOfWeek + (int)firstDayOfWeek);
                        DateTime endOfWeek = startOfWeek.AddDays(6);


                        if (item.CreatedAt.Year == now.Year)
                        {
                            yearCash += item.ParkingZone.PricePerHour *
                                        (decimal)timeDifference.TotalHours;

                            if (item.CreatedAt.Month == now.Month)
                            {
                                monthCash += item.ParkingZone.PricePerHour *
                                             (decimal)timeDifference.TotalHours;

                                if (item.CreatedAt >= startOfWeek && item.CreatedAt <= endOfWeek)
                                {
                                    weekCash += item.ParkingZone.PricePerHour *
                                                (decimal)timeDifference.TotalHours;

                                    if (item.CreatedAt.Day == now.Day)
                                    {
                                        dayCash += item.ParkingZone.PricePerHour *
                                                   (decimal)timeDifference.TotalHours;

                                        if (item.CreatedAt.Hour == now.Hour)
                                        {
                                            hourCash += item.ParkingZone.PricePerHour *
                                                        (decimal)timeDifference.TotalHours;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    var metadata = new
                    {
                        doneCheckInOut = doneCheckInOut,
                        notCheckIn = notCheckIn,
                        notCheckOut = notCheckOut,
                        total = doneCheckInOut + notCheckOut + notCheckIn,
                        hourCash = hourCash,
                        dayCash = dayCash,
                        weekCash = weekCash,
                        monthCash = monthCash,
                        yearCash = yearCash,
                        //Data = list
                    };

                    return Ok(metadata);
                }

            }
            catch (Exception ex)
            {
                return NotFound(ex);
            }
        }
    }
}