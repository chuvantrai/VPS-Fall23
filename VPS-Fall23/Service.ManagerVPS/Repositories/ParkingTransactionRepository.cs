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

        public async Task<int> GetRemainingSlot(Guid parkingZoneId)
        {
            return await GetRemainingSlot(parkingZoneId, DateTime.Now);
        }

        public async Task<int> GetRemainingSlot(Guid parkingZoneId, DateTime checkAt)
        {
            return await this.entities
                 .Where(p => p.ParkingZoneId == parkingZoneId
                 && p.CheckinAt <= checkAt && p.CheckoutAt >= checkAt)
                 .CountAsync();
        }

        public bool IsAlreadyBooking(ParkingTransaction parkingTransaction)
        {
            var transFound = this.entities
                .FirstOrDefault(pt => pt.ParkingZoneId == parkingTransaction.ParkingZoneId
                && pt.LicensePlate.Equals(parkingTransaction.LicensePlate)
                );

            throw new NotImplementedException();
        }

        public async Task<string> CheckLicesePlate(string licenseplate, DateTime checkAt, Guid checkBy)
        {
            var transaction = await entities.Include(t => t.ParkingTransactionDetails)
                .FirstOrDefaultAsync(pt => pt.LicensePlate.Equals(licenseplate));

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
                ParkingTransaction parkingTransaction = new()
                {
                    Id = Guid.NewGuid(),
                    LicensePlate = licenseplate,
                    CreatedAt = DateTime.Now,
                    StatusId = (int)ParkingTransactionStatusEnum.UNPAY,
                    CheckinAt = DateTime.Now,
                    CheckinBy = checkBy,
                    Email = licenseplate,
                    Phone = licenseplate
                };

                await SaveChange();
                return await CanLicensePlateCheckin(licenseplate, checkAt, checkBy);
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
                    && pt.LicensePlate.Equals(licenseplate));

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
