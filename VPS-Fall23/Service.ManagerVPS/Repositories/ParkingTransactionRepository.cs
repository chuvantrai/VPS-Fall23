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
        /*  public async Task<int?> GetAvailableTime(Guid parkingZoneId)
          {

          }*/

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

        public async Task<string> CanLicensePlateCheckout(CheckLicensePlate? licensePlateCheckOut)
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

        public Task<bool> IsAlreadyBooking(BookingSlot bookingSlot)
        {
            return this.entities.AnyAsync(p => p.ParkingZoneId == bookingSlot.ParkingZoneId
            && p.LicensePlate == bookingSlot.LicensePlate
            && p.CheckinAt == bookingSlot.CheckinAt
            && p.CheckoutAt == bookingSlot.CheckoutAt);
        }
    }
}
