using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Service.ManagerVPS.Constants.Enums;
using Service.ManagerVPS.Constants.Notifications;
using Service.ManagerVPS.DTO.Input;
using Service.ManagerVPS.DTO.Output;
using Service.ManagerVPS.ExternalClients;
using Service.ManagerVPS.Models;
using Service.ManagerVPS.Repositories.Interfaces;

namespace Service.ManagerVPS.Repositories
{
    public class ParkingTransactionRepository : VpsRepository<ParkingTransaction>,
        IParkingTransactionRepository
    {
        readonly IConfiguration configuration;

        public ParkingTransactionRepository(
            FALL23_SWP490_G14Context fall23Swp490G14Context,
            IConfiguration configuration)
            : base(fall23Swp490G14Context)
        {
            this.configuration = configuration;
        }

        public async Task<List<ParkingTransaction>> GetAll()
        {
            return await this.entities.ToListAsync();
        }

        public async Task<int> GetBookedSlot(Guid parkingZoneId)
        {
            return await GetBookedSlot(parkingZoneId, DateTime.Now);
        }

        public async Task<int> GetBookedSlot(Guid parkingZoneId, DateTime checkAt)
        {
            return await this.entities
                .Where(p => p.ParkingZoneId == parkingZoneId
                            && (p.StatusId == (int)ParkingTransactionStatusEnum.BOOKED ||
                                p.StatusId == (int)ParkingTransactionStatusEnum.DEPOSIT))
                .CountAsync();
        }

        public List<ParkingTransaction> GetBookedSlot(string? parkingZoneName, Guid ownerId,
            DateTime? checkAt)
        {
            if (!checkAt.HasValue)
            {
                checkAt = DateTime.Now;
            }

            if (parkingZoneName == null || parkingZoneName.Trim() == "" || parkingZoneName.ToLower().Trim() == "all")
            {
                return this.entities
                    .Include(p => p.ParkingZone)
                    .Include(o => o.ParkingZone.Owner)
                    .Where(p => (p.StatusId == (int)ParkingTransactionStatusEnum.BOOKED)
                                && (!p.ParkingTransactionDetails.Any())
                                && (p.ParkingZone.OwnerId == ownerId)
                    ).ToList();
            }

            return this.entities
                .Include(p => p.ParkingZone)
                .Where(p => p.ParkingZone.Name == parkingZoneName
                            && (p.StatusId == (int)ParkingTransactionStatusEnum.BOOKED)
                            && (!p.ParkingTransactionDetails.Any())
                ).ToList();
            ;
        }

        //public async Task<int> GetMonthDoneTransaction(Guid parkingZoneId)
        //{
        //    DateTime dateTime = DateTime.Now;
        //    return await this.entities
        //         .Where(p => p.ParkingZoneId == parkingZoneId
        //         && p.CreatedAt.Month == dateTime.Month
        //         && (p.StatusId == 5)
        //         && (!p.ParkingTransactionDetails.Any()
        //         || p.ParkingTransactionDetails.OrderByDescending(pt => pt.CreatedAt).First().To >= checkAt
        //         ))
        //         .CountAsync();
        //}

        public async Task<string> CheckLicesePlate(string licenseplate, DateTime checkAt,
            Guid checkBy)
        {
            var transaction = await entities.Include(pt => pt.ParkingTransactionDetails)
                .Include(pt => pt.ParkingZone)
                .FirstOrDefaultAsync(pt => pt.LicensePlate.Equals(licenseplate)
                                           && pt.StatusId != (int)ParkingTransactionStatusEnum.PAYED
                                           && pt.CheckinAt <= checkAt
                                           && pt.ParkingZone.ParkingZoneAttendants.Any(p =>
                                               p.Id == checkBy));

            if (transaction != null)
            {
                var transCount = transaction.ParkingTransactionDetails.Count;

                return transCount switch
                {
                    0 => await CanLicensePlateCheckin(licenseplate, checkAt, checkBy, 0),
                    _ => await CanLicensePlateCheckout(licenseplate, checkAt, checkBy),
                };
            }
            else
            {
                var parkingZone = context.ParkingZoneAttendants
                    .Include(parkingZoneAttendant => parkingZoneAttendant.ParkingZone)
                    .FirstOrDefault(pz => pz.Id == checkBy)?.ParkingZone;
                if (parkingZone != null)
                {
                    if (parkingZone.Slots - await GetBookedSlot(parkingZone.Id, checkAt) > 0)
                    {
                        ParkingTransaction parkingTransaction = new()
                        {
                            Id = Guid.NewGuid(),
                            LicensePlate = licenseplate,
                            CreatedAt = DateTime.Now,
                            StatusId = (int)ParkingTransactionStatusEnum.DEPOSIT,
                            ParkingZone = parkingZone,
                            ParkingZoneId = parkingZone.Id,
                            CheckinAt = checkAt,
                            CheckoutAt = checkAt,
                            CheckinBy = checkBy,
                            Email = null,
                            Phone = null
                        };

                        await this.Create(parkingTransaction);
                        await SaveChange();
                        return await CanLicensePlateCheckin(licenseplate, checkAt, checkBy, 1);
                    }
                    else
                    {
                        return ResponseNotification.BOOKING_ERROR;
                    }
                }
                else
                {
                    return ResponseNotification.ATTENDANT_ERROR;
                }
            }
        }

        public async Task<string> CanLicensePlateCheckin(string licenseplate, DateTime checkAt,
            Guid checkBy, int isOutSide)
        {
            if (licenseplate != null)
            {
                ParkingTransaction? transaction = new();
                switch (isOutSide)
                {
                    case 1:
                        transaction = await entities.Include(t => t.ParkingZone)
                            .Include(t => t.ParkingTransactionDetails)
                            .FirstOrDefaultAsync(pt =>
                                pt.StatusId != (int)ParkingTransactionStatusEnum.PAYED
                                && pt.LicensePlate.Equals(licenseplate)
                                && pt.CheckinAt <= checkAt
                                && pt.ParkingZone.ParkingZoneAttendants.Any(p => p.Id == checkBy));
                        break;

                    default:
                        transaction = await entities.Include(t => t.ParkingZone)
                            .Include(t => t.ParkingTransactionDetails)
                            .FirstOrDefaultAsync(pt =>
                                pt.StatusId != (int)ParkingTransactionStatusEnum.PAYED
                                && pt.LicensePlate.Equals(licenseplate)
                                && pt.CheckinAt <= checkAt && pt.CheckoutAt >= checkAt
                                && pt.ParkingZone.ParkingZoneAttendants.Any(p => p.Id == checkBy));
                        break;
                }

                ;

                if (transaction != null)
                {
                    if (checkAt - transaction.CheckinAt < TimeSpan.FromMinutes(15))
                    {
                        var transactionDetail = new ParkingTransactionDetail
                        {
                            Id = Guid.NewGuid(),
                            From = checkAt,
                            To = transaction.CheckoutAt ?? DateTime.Now,
                            ParkingTransaction = transaction,
                            CreatedAt = DateTime.Now,
                            ParkingTransactionId = transaction.Id,
                            UnitPricePerHour = transaction.ParkingZone.PricePerHour,
                            Detail = "CHECK IN AT " + checkAt
                        };

                        context.ParkingTransactionDetails.Add(transactionDetail);
                        transaction.CheckinBy = checkBy;
                        await Update(transaction);
                        await SaveChange();
                        var brokerApiClient = new BrokerApiClient(configuration.GetValue<string>("brokerApiBaseUrl"));
                        await brokerApiClient.RemoveCancelBookingJob(transaction.Id);
                        return ResponseNotification.CHECKIN_SUCCESS;
                    }
                    else
                    {
                        transaction.StatusId = (int)ParkingTransactionStatusEnum.PARKINGCANCEL;
                        await Update(transaction);
                        await SaveChange();
                        return ResponseNotification.OVERTIME_ERROR;
                    }
                }
                else
                {
                    return ResponseNotification.CHECKIN_ERROR;
                }
            }
            else
            {
                return ResponseNotification.CHECKIN_ERROR;
            }
        }

        public async Task<string> CanLicensePlateCheckout(string licenseplate, DateTime checkAt,
            Guid checkBy)
        {
            if (licenseplate != null)
            {
                var transaction = await entities.Include(t => t.ParkingZone)
                    .Include(t => t.ParkingTransactionDetails)
                    .FirstOrDefaultAsync(pt =>
                        pt.StatusId != (int)ParkingTransactionStatusEnum.PAYED
                        && pt.LicensePlate.Equals(licenseplate) &&
                        pt.ParkingZone.ParkingZoneAttendants.Any(p => p.Id == checkBy));

                if (transaction != null)
                {
                    var transactionDetail = transaction.ParkingTransactionDetails
                        .OrderBy(pt => pt.CreatedAt)
                        .FirstOrDefault();

                    if (transactionDetail != null && transaction.StatusId ==
                        (int)ParkingTransactionStatusEnum.BOOKED)
                    {
                        if (transactionDetail.To < checkAt)
                        {
                            var newTransactionDetail = new ParkingTransactionDetail
                            {
                                Id = Guid.NewGuid(),
                                From = transactionDetail.To,
                                To = checkAt,
                                ParkingTransaction = transaction,
                                CreatedAt = DateTime.Now,
                                ParkingTransactionId = transaction.Id,
                                UnitPricePerHour = transaction.ParkingZone.PriceOverTimePerHour,
                                Detail = "CHECK OUT AT " + checkAt
                            };

                            context.ParkingTransactionDetails.Add(newTransactionDetail);
                            transaction.CheckoutBy = checkBy;
                            transaction.StatusId = (int)ParkingTransactionStatusEnum.UNPAY;
                            await Update(transaction);

                            if(checkAt - transaction.CheckoutAt < TimeSpan.FromMinutes(60))
                            {
                                return ResponseNotification.OVERTIME_CONFIRM +
                                   (double)transactionDetail.UnitPricePerHour + " VNĐ";
                            }
                            else
                            {
                                return ResponseNotification.OVERTIME_CONFIRM +
                                   (int)((double)newTransactionDetail.UnitPricePerHour *
                                         (checkAt - transactionDetail.To).TotalHours) + " VNĐ";
                            }
                            
                        }
                        else
                        {
                            transactionDetail.To = checkAt;
                            transactionDetail.Detail = "CHECK OUT AT " + checkAt;
                            context.ParkingTransactionDetails.Update(transactionDetail);

                            transaction.StatusId = (int)ParkingTransactionStatusEnum.PAYED;
                            await Update(transaction);
                        }

                        await SaveChange();
                        return ResponseNotification.CHECKOUT_SUCCESS;
                    }
                    else if (transactionDetail != null && transaction.StatusId ==
                             (int)ParkingTransactionStatusEnum.DEPOSIT)
                    {
                        transactionDetail.To = checkAt;
                        transactionDetail.Detail = "CHECK OUT AT " + checkAt;
                        context.ParkingTransactionDetails.Update(transactionDetail);

                        transaction.CheckoutBy = checkBy;
                        transaction.StatusId = (int)ParkingTransactionStatusEnum.UNPAY;
                        transaction.CheckoutAt = checkAt;
                        await Update(transaction);

                        await SaveChange();

                        if (checkAt - transactionDetail.From < TimeSpan.FromMinutes(60))
                        {
                            return ResponseNotification.OVERTIME_CONFIRM +
                               (double)transactionDetail.UnitPricePerHour + " VNĐ";
                        }
                        else
                        {
                            return ResponseNotification.OVERTIME_CONFIRM +
                               (int)((double)transactionDetail.UnitPricePerHour *
                                     (checkAt - transactionDetail.From).TotalHours) + " VNĐ";
                        }
                    }
                    else if (transactionDetail != null && transaction.StatusId ==
                             (int)ParkingTransactionStatusEnum.UNPAY)
                    {
                        if (transactionDetail.To - transactionDetail.From < TimeSpan.FromMinutes(60))
                        {
                            return ResponseNotification.OVERTIME_CONFIRM +
                               (double)transactionDetail.UnitPricePerHour + " VNĐ";
                        }
                        else
                        {
                            return ResponseNotification.OVERTIME_CONFIRM +
                               (int)((double)transactionDetail.UnitPricePerHour *
                                     (transactionDetail.To - transactionDetail.From).TotalHours) + " VNĐ";
                        }                        
                    }
                    else
                    {
                        return ResponseNotification.CHECKOUT_ERROR;
                    }
                }
                else
                {
                    return ResponseNotification.CHECKOUT_ERROR;
                }
            }
            else
            {
                return ResponseNotification.CHECKOUT_ERROR;
            }
        }

        public async Task<string> CheckOutConfirm(string licenseplate, DateTime checkAt,
            Guid checkBy)
        {
            if (licenseplate != null)
            {
                var transaction = await entities.Include(t => t.ParkingZone)
                    .Include(t => t.ParkingTransactionDetails)
                    .FirstOrDefaultAsync(pt =>
                        pt.StatusId == (int)ParkingTransactionStatusEnum.UNPAY
                        && pt.LicensePlate.Equals(licenseplate) &&
                        pt.ParkingZone.ParkingZoneAttendants.Any(p => p.Id == checkBy));

                if (transaction != null)
                {
                    transaction.StatusId = (int)ParkingTransactionStatusEnum.PAYED;
                    await Update(transaction);
                    await SaveChange();
                    return ResponseNotification.CONFIRM_SUCCESS;
                }
                else
                {
                    return ResponseNotification.CONFIRM_ERROR;
                }
            }
            else
            {
                return ResponseNotification.CONFIRM_ERROR;
            }
        }


        public Task<bool> IsAlreadyBooking(BookingSlot bookingSlot)
        {
            return this.entities.AnyAsync(p =>
                p.LicensePlate == bookingSlot.LicensePlate
                && (p.StatusId == (int)ParkingTransactionStatusEnum.BOOKED
                    || p.StatusId == (int)ParkingTransactionStatusEnum.DEPOSIT)
                && ((bookingSlot.CheckinAt >= p.CheckinAt && bookingSlot.CheckinAt <= p.CheckoutAt)
                    || (bookingSlot.CheckoutAt >= p.CheckinAt &&
                        bookingSlot.CheckoutAt <= p.CheckoutAt))
                && !p.ParkingTransactionDetails.Any());
        }
        public async Task<dynamic> GetParkingTransactionByIdEmail(Guid id, string email)
        {
            var parkingTransactions = await context.ParkingTransactions
                .Include(x => x.ParkingZone)
                .Include(x => x.PaymentTransactions)
                .Where(x => x.ParkingZoneId.Equals(id)
                            && x.ParkingZone.IsApprove == true
                ).ToListAsync();
            if (parkingTransactions.Count == 0)
            {
                return new
                {
                    ParkingTransaction = parkingTransactions,
                    Id = 5014
                };
            }

            var parkingTransaction = parkingTransactions.FirstOrDefault(x => x.Email == email);

            if (parkingTransaction == null)
            {
                return new
                {
                    ParkingTransaction = parkingTransactions,
                    Id = 5008
                };
            }

            return new
            {
                ParkingTransaction = parkingTransaction,
                Id = 200
            };
        }

        public async Task<List<IncomeParkingZoneResponse>> GetAllIncomeByParkingZoneId(
            Guid parkingZoneId)
        {
            var result = new List<IncomeParkingZoneResponse>();
            var parkingTransactions = await entities.Include(pt => pt.ParkingTransactionDetails)
                .Where(pt =>
                    pt.ParkingZoneId == parkingZoneId && pt.CheckoutBy != null &&
                    pt.CheckinBy != null).ToListAsync();
            foreach (var parkingTransaction in parkingTransactions)
            {
                decimal totalCost = 0;

                foreach (var parkingTransactionDetail in parkingTransaction
                             .ParkingTransactionDetails)
                {
                    var duration = parkingTransactionDetail.To - parkingTransactionDetail.From;
                    var totalHours = duration.TotalHours;

                    decimal costForDetail =
                        (decimal)totalHours * parkingTransactionDetail.UnitPricePerHour;
                    totalCost += costForDetail;
                }

                var incomeParkingZoneResponse = new IncomeParkingZoneResponse()
                {
                    Income = totalCost,
                    IncomeDate = parkingTransaction.CheckinAt.Date
                };

                result.Add(incomeParkingZoneResponse);
            }

            return result;
        }

        public List<ParkingTransaction> GetParkingTransactions()
        {
            var lstParkingTransaction = entities.ToList();
            return lstParkingTransaction;
        }

        public async Task SendBookedEmail(ParkingTransaction parkingTransaction,
            PaymentTransaction paymentTransaction)
        {
            var parkingZone = this.context.ParkingZones.Find(parkingTransaction.ParkingZoneId);
            var fileName = $"booking-info.html";
            fileName = Path.Combine(Directory.GetCurrentDirectory(), "Constants", "FileHtml",
                fileName);
            var templateString = await File.ReadAllTextAsync(fileName);
            templateString = templateString
                .Replace("@{parkingZoneName}", parkingTransaction.ParkingZone.Name)
                .Replace("@{transactionCode}", paymentTransaction.TxnRef)
                .Replace("@{Vnp_Amount}", paymentTransaction.Amount.ToString())
                .Replace("@{from}", parkingTransaction.CheckinAt.ToString("hh:mm:ss dd/MM/yyyy"))
                .Replace("@{to}", parkingTransaction.CheckoutAt?.ToString("hh:mm:ss dd/MM/yyyy"))
                .Replace("@{Vnp_OrderInfo}", paymentTransaction.OrderInfo);
            string subject = "Đăng ký gửi xe thành công";
            BrokerApiClient brokerApiClient =
                new BrokerApiClient(this.configuration.GetValue<string>("brokerApiBaseUrl"));
            await brokerApiClient.SendMail(new string[1] { parkingTransaction.Email }, subject,
                templateString);
        }
    }
}