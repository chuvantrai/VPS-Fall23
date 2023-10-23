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

        public async Task<string> CheckLicesePlate(CheckLicensePlate checkLicensePlate)
        {
            var transaction = await entities
                .FirstOrDefaultAsync(pt => pt.StatusId.Equals(ParkingTransactionStatusEnum.BOOKED)
                || pt.StatusId.Equals(ParkingTransactionStatusEnum.BOOKED)
                && pt.LicensePlate.Equals(checkLicensePlate.LicensePlate));

            if (transaction != null)
            {
                var transCount = transaction.ParkingTransactionDetails.Count;

                return transCount switch
                {
                    0 => await CanLicensePlateCheckin(checkLicensePlate),
                    _ => await CanLicensePlateCheckout(checkLicensePlate),
                };
            }
            else
            {
                throw new NotImplementedException();
            }

        }

        public async Task<string> CanLicensePlateCheckin(CheckLicensePlate? licensePlateCheckIn)
        {
            if (licensePlateCheckIn != null)
            {
                var transaction = await entities
                    .FirstOrDefaultAsync(pt => pt.StatusId.Equals(ParkingTransactionStatusEnum.BOOKED)
                    || pt.StatusId.Equals(ParkingTransactionStatusEnum.BOOKED)
                    && pt.LicensePlate.Equals(licensePlateCheckIn.LicensePlate));

                if (transaction != null)
                {
                    var transactionDetail = new ParkingTransactionDetail
                    {
                        From = licensePlateCheckIn.CheckAt,
                        To = transaction.CheckoutAt ?? DateTime.Now,
                        ParkingTransaction = transaction,
                        CreatedAt = DateTime.Now,
                        ParkingTransactionId = transaction.Id,
                        UnitPricePerHour = transaction.ParkingZone.PricePerHour,
                        Detail = "CHECK IN AT " + licensePlateCheckIn.CheckAt
                    };

                    context.ParkingTransactionDetails.Add(transactionDetail);
                    transaction.CheckinBy = licensePlateCheckIn.CheckBy;
                    await Update(transaction);
                    context.SaveChanges();

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

        public async Task <string> CanLicensePlateCheckout(CheckLicensePlate? licensePlateCheckOut)
        {
            if (licensePlateCheckOut != null)
            {
                var transaction = await entities
                    .FirstOrDefaultAsync(pt => pt.StatusId.Equals(ParkingTransactionStatusEnum.BOOKED)
                    || pt.StatusId.Equals(ParkingTransactionStatusEnum.BOOKED)
                    && pt.LicensePlate.Equals(licensePlateCheckOut.LicensePlate));

                if (transaction != null)
                {
                    var transactionDetail = transaction.ParkingTransactionDetails
                        .OrderBy(pt => pt.CreatedAt)
                        .FirstOrDefault();

                    if (transactionDetail != null)
                    {
                        if (transactionDetail.To < licensePlateCheckOut.CheckAt)
                        {
                            var newTransactionDetail = new ParkingTransactionDetail
                            {
                                From = transactionDetail.To,
                                To = licensePlateCheckOut.CheckAt,
                                ParkingTransaction = transaction,
                                CreatedAt = DateTime.Now,
                                ParkingTransactionId = transaction.Id,
                                UnitPricePerHour = transaction.ParkingZone.PricePerHour,
                                Detail = "CHECK OUT AT " + licensePlateCheckOut.CheckAt
                            };

                            context.ParkingTransactionDetails.Add(transactionDetail);
                            transaction.CheckoutBy = licensePlateCheckOut.CheckBy;
                            await Update(transaction);
                        }
                        else
                        {
                            transactionDetail.To = licensePlateCheckOut.CheckAt;
                            transactionDetail.Detail = "CHECK OUT AT " + licensePlateCheckOut.CheckAt;
                            context.ParkingTransactionDetails.Update(transactionDetail);
                        }
                    }

                    context.SaveChanges();

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
