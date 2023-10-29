using Microsoft.EntityFrameworkCore;
using Service.ManagerVPS.Constants.Enums;
using Service.ManagerVPS.Constants.Notifications;
using Service.ManagerVPS.DTO.Input;
using Service.ManagerVPS.Models;
using Service.ManagerVPS.Repositories.Interfaces;

namespace Service.ManagerVPS.Repositories
{
    public class ParkingTransactionRepository : VpsRepository<ParkingTransaction>, IParkingTransactionRepository
    {
        public ParkingTransactionRepository(FALL23_SWP490_G14Context fALL23_SWP490_G14Context)
            : base(fALL23_SWP490_G14Context)
        {

        }

        public async Task<int> GetBookedSlot(Guid parkingZoneId)
        {
            return await GetBookedSlot(parkingZoneId, DateTime.Now);
        }

        public async Task<int> GetBookedSlot(Guid parkingZoneId, DateTime checkAt)
        {
            return await this.entities
                 .Where(p => p.ParkingZoneId == parkingZoneId
                 && p.CheckinAt <= checkAt
                 && p.CheckoutAt >= checkAt
                 && p.StatusId == (int)ParkingTransactionStatusEnum.BOOKED
                 && (!p.ParkingTransactionDetails.Any()
                 || p.ParkingTransactionDetails.OrderByDescending(pt => pt.CreatedAt).First().To >= checkAt
                 ))
                 .CountAsync();
        }

        public async Task<string> CheckLicesePlate(string licenseplate, DateTime checkAt, Guid checkBy)
        {
            var transaction = await entities.Include(t => t.ParkingTransactionDetails)
                .FirstOrDefaultAsync(pt => pt.LicensePlate.Equals(licenseplate)
                && pt.CheckinAt <= checkAt && pt.CheckoutAt >= checkAt
                && pt.ParkingZone.ParkingZoneAttendants.Any(p => p.Id == checkBy));

            if (transaction != null)
            {
                var transCount = transaction.ParkingTransactionDetails.Count;

                return transCount switch
                {
                    0 => await CanLicensePlateCheckin(licenseplate, checkAt, checkBy),
                    _ => await CanLicensePlateCheckout(licenseplate, checkAt, checkBy),
                };
            }
            else
            {
                var parkingZone = context.ParkingZoneAttendants.FirstOrDefault(pz => pz.Id == checkBy)?.ParkingZone;
                if (parkingZone != null)
                {
                    ParkingTransaction parkingTransaction = new()
                    {
                        Id = Guid.NewGuid(),
                        LicensePlate = licenseplate,
                        CreatedAt = DateTime.Now,
                        StatusId = (int)ParkingTransactionStatusEnum.UNPAY,
                        ParkingZone = parkingZone,
                        ParkingZoneId = parkingZone.Id,
                        CheckinAt = DateTime.Now,
                        CheckinBy = checkBy,
                        Email = licenseplate,
                        Phone = licenseplate
                    };

                    await this.Create(parkingTransaction);
                    await SaveChange();
                    return await CanLicensePlateCheckin(licenseplate, checkAt, checkBy);
                }
                else
                {
                    return ResponseNotification.CHECKIN_ERROR;
                }
            }
        }

        public async Task<string> CanLicensePlateCheckin(string licenseplate, DateTime checkAt, Guid checkBy)
        {

            if (licenseplate != null)
            {
                var transaction = await entities.Include(t => t.ParkingZone).Include(t => t.ParkingTransactionDetails)
                    .FirstOrDefaultAsync(pt => pt.StatusId == (int)ParkingTransactionStatusEnum.BOOKED
                    || pt.StatusId == (int)ParkingTransactionStatusEnum.PAYED
                    || pt.StatusId == (int)ParkingTransactionStatusEnum.UNPAY
                    || pt.StatusId == (int)ParkingTransactionStatusEnum.DEPOSIT
                    && pt.LicensePlate.Equals(licenseplate)
                     && pt.CheckinAt <= checkAt && pt.CheckoutAt >= checkAt
                && pt.ParkingZone.ParkingZoneAttendants.Any(p => p.Id == checkBy));

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

                        return ResponseNotification.CHECKIN_SUCCESS;
                    }
                    else
                    {
                        transaction.StatusId = (int)ParkingTransactionStatusEnum.PARKINGCANCEL;
                        await Update(transaction);
                        await SaveChange();
                        return ResponseNotification.CHECKIN_ERROR;
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

        public async Task<string> CanLicensePlateCheckout(string licenseplate, DateTime checkAt, Guid checkBy)
        {
            if (licenseplate != null)
            {
                var transaction = await entities.Include(t => t.ParkingZone).Include(t => t.ParkingTransactionDetails)
                    .FirstOrDefaultAsync(pt => pt.StatusId == (int)ParkingTransactionStatusEnum.BOOKED
                    || pt.StatusId == (int)ParkingTransactionStatusEnum.PAYED
                    || pt.StatusId == (int)ParkingTransactionStatusEnum.UNPAY
                    || pt.StatusId == (int)ParkingTransactionStatusEnum.DEPOSIT
                    && pt.LicensePlate.Equals(licenseplate));

                if (transaction != null)
                {
                    var transactionDetail = transaction.ParkingTransactionDetails
                        .OrderBy(pt => pt.CreatedAt)
                        .FirstOrDefault();

                    if (transactionDetail != null && transaction.StatusId != (int)ParkingTransactionStatusEnum.UNPAY)
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
                                UnitPricePerHour = transaction.ParkingZone.PricePerHour,
                                Detail = "CHECK OUT AT " + checkAt
                            };

                            context.ParkingTransactionDetails.Add(newTransactionDetail);
                            transaction.CheckoutBy = checkBy;
                            await Update(transaction);
                        }
                        else
                        {
                            transactionDetail.To = checkAt;
                            transactionDetail.Detail = "CHECK OUT AT " + checkAt;
                            context.ParkingTransactionDetails.Update(transactionDetail);
                        }

                        await SaveChange();
                        return ResponseNotification.CHECKOUT_SUCCESS;
                    }
                    else if (transaction.StatusId == (int)ParkingTransactionStatusEnum.UNPAY)
                    {
                        return ResponseNotification.CHECKOUT_CONFIRM;
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

        public async Task<string> CheckOutConfirm(string licenseplate, DateTime checkAt, Guid checkBy)
        {
            if (licenseplate != null)
            {
                var transaction = await entities.Include(t => t.ParkingZone).Include(t => t.ParkingTransactionDetails)
                    .FirstOrDefaultAsync(pt => pt.StatusId == (int)ParkingTransactionStatusEnum.UNPAY
                    && pt.LicensePlate.Equals(licenseplate));

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
            return this.entities.AnyAsync(p => p.ParkingZoneId == bookingSlot.ParkingZoneId
            && p.LicensePlate == bookingSlot.LicensePlate
             && p.StatusId == (int)ParkingTransactionStatusEnum.BOOKED
            && ((bookingSlot.CheckinAt >= p.CheckinAt && bookingSlot.CheckinAt <= p.CheckoutAt)
            || (bookingSlot.CheckoutAt >= p.CheckinAt && bookingSlot.CheckoutAt <= p.CheckoutAt))
            && !p.ParkingTransactionDetails.Any());
        }

        public async Task<ParkingTransaction?> GetParkingTransactionByIdEmail(Guid id, string email)
        {
            var parkingTransaction = await context.ParkingTransactions
                .Include(x => x.ParkingZone)
                .Include(x => x.PaymentTransactions)
                .FirstOrDefaultAsync(x => x.ParkingZoneId.Equals(id)
                                          && x.ParkingZone.IsApprove == true
                                          && x.Email == email);
            return parkingTransaction;
        }
    }
}
