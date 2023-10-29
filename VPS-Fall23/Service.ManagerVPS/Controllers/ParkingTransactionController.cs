using Google.Cloud.Vision.V1;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Service.ManagerVPS.Controllers.Base;
using Service.ManagerVPS.DTO.Exceptions;
using Service.ManagerVPS.DTO.Input;
using Service.ManagerVPS.DTO.VNPay;
using Service.ManagerVPS.Extensions.StaticLogic;
using Service.ManagerVPS.ExternalClients.VNPay;
using Service.ManagerVPS.ExternalClients;
using Service.ManagerVPS.Models;
using Service.ManagerVPS.Repositories.Interfaces;
using Service.ManagerVPS.Constants.Enums;

namespace Service.ManagerVPS.Controllers
{
    public class ParkingTransactionController : VpsController<ParkingTransaction>
    {
        private readonly VnPayConfig vnPayConfig;
        private readonly IParkingZoneRepository parkingZoneRepository;
        private readonly IPaymentTransactionRepository paymentTransactionRepository;
        private readonly GoogleApiService _googleApiService;

        public ParkingTransactionController(
            IParkingTransactionRepository parkingTransactionRepository,
            GoogleApiService googleApiService,
            IOptions<VnPayConfig> vnPayConfig,
            IParkingZoneRepository parkingZoneRepository,
            IPaymentTransactionRepository paymentTransaction)
            : base(parkingTransactionRepository)
        {
            this.vnPayConfig = vnPayConfig.Value;
            this.parkingZoneRepository = parkingZoneRepository;
            paymentTransactionRepository = paymentTransaction;
            _googleApiService = googleApiService;
        }
        [HttpPost]
        public async Task<ParkingTransaction> Booking(BookingSlot bookingSlot)
        {
            if (await ((IParkingTransactionRepository)vpsRepository).IsAlreadyBooking(bookingSlot))
            {
                throw new ClientException(1003);
            }
            ParkingZone parkingZone = await parkingZoneRepository.Find(bookingSlot.ParkingZoneId);
            int bookedSlotAtCheckinTime = await ((IParkingTransactionRepository)vpsRepository).GetBookedSlot(parkingZone.Id, bookingSlot.CheckinAt);
            if (parkingZone.Slots - bookedSlotAtCheckinTime <= 0)
            {
                throw new ClientException(1004);
            }
            int bookedSlotAtCheckoutTime = await ((IParkingTransactionRepository)vpsRepository).GetBookedSlot(parkingZone.Id, bookingSlot.CheckoutAt);
            if (parkingZone.Slots - bookedSlotAtCheckoutTime <= 0)
            {
                throw new ClientException(1005);
            }

            ParkingTransaction parkingTransaction = new()
            {
                Id = Guid.NewGuid(),
                ParkingZoneId = bookingSlot.ParkingZoneId,
                CreatedAt = DateTime.Now,
                CheckinAt = bookingSlot.CheckinAt,
                CheckoutAt = bookingSlot.CheckoutAt,
                Email = bookingSlot.Email,
                StatusId = (int)ParkingTransactionStatusEnum.BOOKED,
                Phone = bookingSlot.Phone,
                LicensePlate = bookingSlot.LicensePlate
            };
            parkingTransaction.Id = Guid.NewGuid();
            parkingTransaction.CreatedAt = DateTime.Now;
            ParkingTransaction response = await vpsRepository.Create(parkingTransaction);
            _ = await vpsRepository.SaveChange();
            return response;
        }

        [HttpPost]
        public async Task<string> CheckLicensePlateScan(LicensePlateScan licensePlateScan)
        {
            var image = Image.FromBytes(licensePlateScan.Image) ?? throw new ClientException(3003);

            var licensePlate = await _googleApiService.GetLicensePlateFromImage(image) ?? throw new ClientException(3000);

            if (!GeneralExtension.IsLicensePlateValid(licensePlate))
            {
                throw new ClientException(3001);
            }

            return await ((IParkingTransactionRepository)vpsRepository).CheckLicesePlate(licensePlate, licensePlateScan.CheckAt, licensePlateScan.CheckBy) ?? throw new ClientException(3002);
        }

        [HttpPost]
        public async Task<string> CheckLicensePlateInput(LicensePlateInput licensePlateInput)
        {
            var licensePlate = licensePlateInput.LicensePlate ?? throw new ClientException(3000);

            if (!GeneralExtension.IsLicensePlateValid(licensePlate))
            {
                throw new ClientException(3001);
            }

            return await ((IParkingTransactionRepository)vpsRepository).CheckLicesePlate(licensePlate, licensePlateInput.CheckAt, licensePlateInput.CheckBy) ?? throw new ClientException(3002);
        }
        [HttpGet("{parkingTransactionId}")]
        public async Task<string> GetPayUrl(Guid parkingTransactionId)
        {
            ParkingTransaction transaction = await vpsRepository.Find(parkingTransactionId);
            _ = await parkingZoneRepository.Find(transaction.ParkingZoneId);
            decimal totalMoney = (decimal)(transaction.CheckoutAt - transaction.CheckinAt).Value.TotalHours * transaction.ParkingZone.PricePerHour;
            string orderInfo = $"Thanh toan gui xe bien so {transaction.LicensePlate} tu {transaction.CheckinAt} den {transaction.CheckoutAt}";
            string url = new VNPayClient(vnPayConfig)
                .InitRequestParams(GetIpAddress(),
                out string txnRef)
                .CreateRequestUrl(vnPayConfig.Url,
                totalMoney,
                vnPayConfig.ExpireMinutes,
                out string secureHash,
             orderInfo);
            PaymentTransaction paymentTransaction = new()
            {
                BookingId = parkingTransactionId,
                TxnRef = txnRef,
                Amount = totalMoney,
                OrderInfo = orderInfo,
                SecureHash = secureHash,
            };
            _ = await paymentTransactionRepository.Create(paymentTransaction);
            _ = await paymentTransactionRepository.SaveChange();
            return url;
        }

        [HttpPost]
        public async Task<string> CheckOutConfirm(LicensePlateInput licensePlateInput)
        {
            var licensePlate = licensePlateInput.LicensePlate ?? throw new ClientException(3000);

            if (!GeneralExtension.IsLicensePlateValid(licensePlate))
            {
                throw new ClientException(3001);
            }

            return await ((IParkingTransactionRepository)vpsRepository).CheckOutConfirm(licensePlate, licensePlateInput.CheckAt, licensePlateInput.CheckBy) ?? throw new ClientException(3002);
        }
    }
}
