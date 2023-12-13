using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public IActionResult GetBookedOverview([FromQuery] string? parkingZoneId)
        {
            try
            {
                var accessToken = Request.Cookies["ACCESS_TOKEN"]!;
                var userToken = JwtTokenExtension.ReadToken(accessToken)!;

                var ownerId = userToken.UserId;

                DateTime now = DateTime.Now;
                var list = parkingTransactionRepository.GetBookedSlot(parkingZoneId, DateTime.Now);
                using (var context = new FALL23_SWP490_G14Context())
                {
                    var test = context.ParkingTransactions.Where(p => (p.ParkingZone.Id.ToString().Equals(parkingZoneId))).ToList();

                    var bookedList = context.ParkingTransactions
                                        .Include(x => x.ParkingZone)
                                        .Include(o => o.ParkingZone.Owner)
                                        .Where(p => (p.StatusId == (int)ParkingTransactionStatusEnum.BOOKED)
                                            && (string.IsNullOrEmpty(parkingZoneId) || p.ParkingZone.Id.ToString().Equals(parkingZoneId))
                                            && (p.ParkingZone.OwnerId.ToString().Equals(ownerId)))
                                        .ToList();
                    var depositList = context.ParkingTransactions
                                        .Include(x => x.ParkingZone)
                                        .Include(o => o.ParkingZone.Owner)
                                        .Where(p => (p.StatusId == (int)ParkingTransactionStatusEnum.DEPOSIT)
                                            && (string.IsNullOrEmpty(parkingZoneId) || p.ParkingZone.Id.ToString().Equals(parkingZoneId))
                                            && (p.ParkingZone.OwnerId.ToString().Equals(ownerId)))
                                        .ToList();
                    var unPayList = context.ParkingTransactions
                                        .Include(x => x.ParkingZone)
                                        .Include(o => o.ParkingZone.Owner)
                                        .Where(p => (p.StatusId == (int)ParkingTransactionStatusEnum.UNPAY)
                                            && (string.IsNullOrEmpty(parkingZoneId) || p.ParkingZone.Id.ToString().Equals(parkingZoneId))
                                            && (p.ParkingZone.OwnerId.ToString().Equals(ownerId)))
                                        .ToList();
                    var userCancelList = context.ParkingTransactions
                                        .Include(x => x.ParkingZone)
                                        .Include(o => o.ParkingZone.Owner)
                                        .Where(p => (p.StatusId == (int)ParkingTransactionStatusEnum.USERCANCEL)
                                            && (string.IsNullOrEmpty(parkingZoneId) || p.ParkingZone.Id.ToString().Equals(parkingZoneId))
                                            && (p.ParkingZone.OwnerId.ToString().Equals(ownerId)))
                                        .ToList();
                    var parkingCancelList = context.ParkingTransactions
                                        .Include(x => x.ParkingZone)
                                        .Include(o => o.ParkingZone.Owner)
                                        .Where(p => (p.StatusId == (int)ParkingTransactionStatusEnum.PARKINGCANCEL)
                                            && (string.IsNullOrEmpty(parkingZoneId) || p.ParkingZone.Id.ToString().Equals(parkingZoneId))
                                            && (p.ParkingZone.OwnerId.ToString().Equals(ownerId)))
                                        .ToList();
                    var payedList = context.ParkingTransactions
                                        .Include(x => x.ParkingZone)
                                        .Include(o => o.ParkingZone.Owner)
                                        .Where(p => (p.StatusId == (int)ParkingTransactionStatusEnum.PAYED)
                                            && (string.IsNullOrEmpty(parkingZoneId) || p.ParkingZone.Id.ToString().Equals(parkingZoneId))
                                            && (p.ParkingZone.OwnerId.ToString().Equals(ownerId)))
                                        .ToList();
                    var payedFailedList = context.ParkingTransactions
                                        .Include(x => x.ParkingZone)
                                        .Include(o => o.ParkingZone.Owner)
                                        .Where(p => (p.StatusId == (int)ParkingTransactionStatusEnum.BOOKING_PAID_FAILED)
                                            && (string.IsNullOrEmpty(parkingZoneId) || p.ParkingZone.Id.ToString().Equals(parkingZoneId))
                                            && (p.ParkingZone.OwnerId.ToString().Equals(ownerId)))
                                        .ToList();

                    DayOfWeek
                            firstDayOfWeek =
                                DayOfWeek
                                    .Monday; // You can adjust this to your preferred first day of the week

                    DateTime startOfWeek = now.AddDays(-(int)now.DayOfWeek + (int)firstDayOfWeek);
                    DateTime endOfWeek = startOfWeek.AddDays(6);

                    var booked = bookedList.Count();
                    var deposit = depositList.Count();
                    var unPay = unPayList.Count(); 
                    var payed = payedList.Count();
                    var userCancel = userCancelList.Count();
                    var parkingCancel = parkingCancelList.Count();
                    var payedFailed = payedFailedList.Count();
                    var total = booked + deposit + unPay + payed + userCancel + payedFailed + parkingCancel;

                    var bookedWeek = bookedList.Where(x => x.CreatedAt >= startOfWeek && x.CreatedAt <= endOfWeek).Count();
                    var depositWeek = depositList.Where(x => x.CreatedAt >= startOfWeek && x.CreatedAt <= endOfWeek).Count();
                    var unPayWeek = unPayList.Where(x => x.CreatedAt >= startOfWeek && x.CreatedAt <= endOfWeek).Count();
                    var payedWeek = payedList.Where(x => x.CreatedAt >= startOfWeek && x.CreatedAt <= endOfWeek).Count();
                    var userCancelWeek = userCancelList.Where(x => x.CreatedAt >= startOfWeek && x.CreatedAt <= endOfWeek).Count();
                    var parkingCancelWeek = parkingCancelList.Where(x => x.CreatedAt >= startOfWeek && x.CreatedAt <= endOfWeek).Count();
                    var payedFailedWeek = payedFailedList.Where(x => x.CreatedAt >= startOfWeek && x.CreatedAt <= endOfWeek).Count();
                    var totalWeek = bookedWeek + depositWeek + unPayWeek + payedWeek + userCancelWeek + payedFailedWeek + parkingCancelWeek;

                    var bookedMonth = bookedList.Where(x => x.CreatedAt.Month == now.Month && x.CreatedAt.Year == now.Year).Count();
                    var depositMonth = depositList.Where(x => x.CreatedAt.Month == now.Month && x.CreatedAt.Year == now.Year).Count();
                    var unPayMonth = unPayList.Where(x => x.CreatedAt.Month == now.Month && x.CreatedAt.Year == now.Year).Count();
                    var payedMonth = payedList.Where(x => x.CreatedAt.Month == now.Month && x.CreatedAt.Year == now.Year).Count();
                    var userCancelMonth = userCancelList.Where(x => x.CreatedAt.Month == now.Month && x.CreatedAt.Year == now.Year).Count();
                    var parkingCancelMonth = parkingCancelList.Where(x => x.CreatedAt.Month == now.Month && x.CreatedAt.Year == now.Year).Count();
                    var payedFailedMonth = payedFailedList.Where(x => x.CreatedAt.Month == now.Month && x.CreatedAt.Year == now.Year).Count();
                    var totalMonth = bookedMonth + depositMonth + unPayMonth + payedMonth + userCancelMonth + payedFailedMonth + parkingCancelMonth;

                    var bookedYear = bookedList.Where(x => x.CreatedAt.Year == now.Year).Count();
                    var depositYear = depositList.Where(x => x.CreatedAt.Year == now.Year).Count();
                    var unPayYear = unPayList.Where(x => x.CreatedAt.Year == now.Year).Count();
                    var payedYear = payedList.Where(x => x.CreatedAt.Year == now.Year).Count();
                    var userCancelYear = userCancelList.Where(x => x.CreatedAt.Year == now.Year).Count();
                    var parkingCancelYear = parkingCancelList.Where(x => x.CreatedAt.Year == now.Year).Count();
                    var payedFailedYear = payedFailedList.Where(x => x.CreatedAt.Year == now.Year).Count();
                    var totalYear = bookedYear + depositYear + unPayYear + payedYear + userCancelYear + payedFailedYear + parkingCancelYear;

                    var metadata = new
                    {
                        booked = booked,
                        deposit = deposit,
                        unPay = unPay,
                        payed = payed,
                        userCancel = userCancel,
                        parkingCancel = parkingCancel,
                        payedFailed = payedFailed,
                        total = total,
                        bookedWeek = bookedWeek,
                        depositWeek = depositWeek,
                        unPayWeek = unPayWeek,
                        payedWeek = payedWeek,
                        userCanceWeekl = userCancelWeek,
                        parkingCancelWeek = parkingCancelWeek,
                        payedFailedWeek = payedFailedWeek,
                        totalWeek = totalWeek,
                        bookedMonth = bookedMonth,
                        depositMonth = depositMonth,
                        unPayMonth = unPayMonth,
                        payedMonth = payedMonth,
                        userCancelMonth = userCancelMonth,
                        parkingCancelMonth = parkingCancelMonth,
                        payedFailedMonth = payedFailedMonth,
                        totalMonth = totalMonth,
                        bookedYear = bookedYear,
                        depositYear = depositYear,
                        unPayYear = unPayYear,
                        payedYear = payedYear,
                        userCancelYear = userCancelYear,
                        parkingCancelYear = parkingCancelYear,
                        payedFailedYear = payedFailedYear,
                        totalYear = totalYear,
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