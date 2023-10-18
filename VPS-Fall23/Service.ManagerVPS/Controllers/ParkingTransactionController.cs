using Google.Cloud.Vision.V1;
using Microsoft.AspNetCore.Mvc;
using Service.ManagerVPS.Controllers.Base;
using Service.ManagerVPS.DTO.Exceptions;
using Service.ManagerVPS.DTO.Input;
using Service.ManagerVPS.Extensions.StaticLogic;
using Service.ManagerVPS.ExternalClients;
using Service.ManagerVPS.Models;
using Service.ManagerVPS.Repositories;
using Service.ManagerVPS.Repositories.Interfaces;

namespace Service.ManagerVPS.Controllers
{
    public class ParkingTransactionController : VpsController<ParkingTransaction>
    {
        private readonly GoogleApiService _googleApiService;

        public ParkingTransactionController(IParkingTransactionRepository parkingTransactionRepository, GoogleApiService googleApiService)
            : base(parkingTransactionRepository)
        {
            _googleApiService = googleApiService;
        }

        [HttpPost]
        public async Task<ParkingTransaction> Booking(BookingSlot bookingSlot)
        {
            //if (((IParkingTransactionRepository)vpsRepository).IsAlreadyBooking(parkingTransaction))
            //{
            //    throw new ClientException(1003);
            //}
            ParkingTransaction parkingTransaction = new ParkingTransaction()
            {
                Id = Guid.NewGuid(),
                ParkingZoneId = bookingSlot.ParkingZoneId,
                CreatedAt = DateTime.Now,
                CheckinAt = bookingSlot.CheckinAt,
                CheckoutAt = bookingSlot.CheckoutAt,
                Email = bookingSlot.Email,
                Phone = bookingSlot.Phone,
                LicensePlate = bookingSlot.LicensePlate
            };
            parkingTransaction.Id = Guid.NewGuid();
            parkingTransaction.CreatedAt = DateTime.Now;
            var response = await this.vpsRepository.Create(parkingTransaction);
            await this.vpsRepository.SaveChange();
            return response;
        }

        [HttpPost]
        public async Task<string> CheckLicensePlate(LicensePlateInfo licensePlateInfo)
        {
            var image = Image.FromBytes(licensePlateInfo.Image) ?? throw new ClientException(3003);

            var licensePlate = await _googleApiService.GetLicensePlateFromImage(image) ?? throw new ClientException(3000);

            if (!GeneralExtension.IsLicensePlateValid(licensePlate))
            {
                throw new ClientException(3001);
            }

            return await ((IParkingTransactionRepository)vpsRepository).CheckLicesePlate(licensePlate,licensePlateInfo) ?? throw new ClientException(3002);
        }
    }
}
