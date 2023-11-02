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
                .FirstOrDefaultAsync(pt => pt.StatusId == (int)ParkingTransactionStatusEnum.BOOKED
                || pt.StatusId == (int)ParkingTransactionStatusEnum.DEPOSIT
                && pt.LicensePlate.Equals(licenseplate));

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
                return ResponseNotification.NO_DATA;
            }

        }

        public async Task<string> CanLicensePlateCheckin(string licenseplate, DateTime checkAt, Guid checkBy)
        {

            if (licenseplate != null)
            {
                var transaction = await entities.Include(t => t.ParkingZone).Include(t => t.ParkingTransactionDetails)
                    .FirstOrDefaultAsync(pt => pt.StatusId == (int)ParkingTransactionStatusEnum.BOOKED
                    || pt.StatusId == (int)ParkingTransactionStatusEnum.DEPOSIT
                    && pt.LicensePlate.Equals(licenseplate));

                if (transaction != null)
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
                    || pt.StatusId == (int)ParkingTransactionStatusEnum.DEPOSIT
                    && pt.LicensePlate.Equals(licenseplate));

                if (transaction != null)
                {
                    var transactionDetail = transaction.ParkingTransactionDetails
                        .OrderBy(pt => pt.CreatedAt)
                        .FirstOrDefault();

                    if (transactionDetail != null)
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
                    }

                    await SaveChange();
                    return ResponseNotification.CHECKOUT_SUCCESS;
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

        public async Task<dynamic> GetParkingTransactionByIdEmail(Guid id, string email)
        {
            var parkingTransactions = await context.ParkingTransactions
                .Include(x => x.ParkingZone)
                .Include(x => x.PaymentTransactions)
                .Where(x => x.ParkingZoneId.Equals(id)
                                          && x.ParkingZone.IsApprove == true
                                          && x.Email == email).ToListAsync();
            if (parkingTransactions.Count == 0)
            {
                return new
                {
                    ParkingTransaction = parkingTransactions,
                    Id = 5014
                };
            }
            var parkingTransaction = parkingTransactions.FirstOrDefault(x => x.Email == email);

            if (parkingTransaction != null)
            {
                return new
                {
                    ParkingTransaction = parkingTransactions,
                    id = 5010
                };
            }
            
            return new
            {
                ParkingTransaction = parkingTransaction,
                Id = 200
            };
        }
    }
}
