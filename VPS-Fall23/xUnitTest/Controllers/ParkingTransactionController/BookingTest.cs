using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Options;
using Service.ManagerVPS.DTO.Exceptions;
using Service.ManagerVPS.DTO.Input;
using Service.ManagerVPS.DTO.VNPay;
using Service.ManagerVPS.ExternalClients;
using Service.ManagerVPS.Models;
using Service.ManagerVPS.Repositories;
using Service.ManagerVPS.Repositories.Interfaces;
using Xunit.Abstractions;

namespace xUnitTest.Controllers.ParkingTransactionController
{
    public class BookingTest : GlobalBase
    {
        readonly Service.ManagerVPS.Controllers.ParkingTransactionController parkingTransactionController;
        readonly GoogleApiService googleApiService = FakeItEasy.A.Fake<GoogleApiService>();
        readonly IOptions<VnPayConfig> options = FakeItEasy.A.Fake<IOptions<VnPayConfig>>();
        readonly IPaymentTransactionRepository paymentTransactionRepository = FakeItEasy.A.Fake<IPaymentTransactionRepository>();
        protected Guid parkingZoneIdHaveBookedSlot = Guid.NewGuid();
        protected Guid parkingZoneIdNotHaveBookedSlot = Guid.NewGuid();
        protected DateTime TimeHaveBookedSlot = DateTime.Now;
        protected DateTime TimeDoNotHaveBookedSlot = DateTime.Now.AddDays(-1);
        protected DateTime checkAt = DateTime.Now;
        IEnumerable<ParkingTransaction> GetTestData()
        {
            var phone = "0963602512";
            var email = "nghia@vps.com";
            return (new List<ParkingTransaction>()
            {
                new ParkingTransaction()
                {
                    Id = Guid.NewGuid(),
                    ParkingZoneId = parkingZoneIdHaveBookedSlot,
                    CheckinAt = TimeHaveBookedSlot,
                    CheckoutAt = TimeHaveBookedSlot.AddHours(2),
                    Phone = phone,
                    Email = email,
                    LicensePlate = "29X3-12345",
                    StatusId = 1,
                    CreatedAt = TimeHaveBookedSlot,


                },
                new ParkingTransaction()
                {
                    Id = Guid.NewGuid(),
                    ParkingZoneId = parkingZoneIdHaveBookedSlot,
                    CheckinAt = TimeHaveBookedSlot,
                    CheckoutAt = TimeHaveBookedSlot.AddHours(2),
                    Phone = phone,
                    Email = email,
                    LicensePlate = "29X3-12345",
                    StatusId = 1,
                    CreatedAt = TimeHaveBookedSlot,


                },
                new ParkingTransaction()
                {
                    Id = Guid.NewGuid(),
                    ParkingZoneId = parkingZoneIdHaveBookedSlot,
                    CheckinAt = TimeHaveBookedSlot,
                    CheckoutAt = TimeHaveBookedSlot.AddHours(2),
                    Phone = phone,
                    Email = email,
                    LicensePlate = "29X3-12345",
                    StatusId = 1,
                    CreatedAt = TimeHaveBookedSlot,


                },
                new ParkingTransaction()
                {
                    Id = Guid.NewGuid(),
                    ParkingZoneId = parkingZoneIdHaveBookedSlot,
                    CheckinAt = TimeHaveBookedSlot,
                    CheckoutAt = TimeHaveBookedSlot.AddHours(2),
                    Phone = phone,
                    Email = email,
                    LicensePlate = "29X3-12345",
                    StatusId = 1,
                    CreatedAt = TimeHaveBookedSlot,


                },
                new ParkingTransaction()
                {
                    Id = Guid.NewGuid(),
                    ParkingZoneId = parkingZoneIdHaveBookedSlot,
                    CheckinAt = TimeHaveBookedSlot,
                    CheckoutAt = TimeHaveBookedSlot.AddHours(2),
                    Phone = phone,
                    Email = email,
                    LicensePlate = "29X3-12345",
                    StatusId = 1,
                    CreatedAt = TimeHaveBookedSlot,


                },
                new ParkingTransaction()
                {
                    Id = Guid.NewGuid(),
                    ParkingZoneId = parkingZoneIdHaveBookedSlot,
                    CheckinAt = TimeHaveBookedSlot,
                    CheckoutAt = TimeHaveBookedSlot.AddHours(2),
                    Phone = phone,
                    Email = email,
                    LicensePlate = "29X3-12345",
                    StatusId = 1,
                    CreatedAt = TimeHaveBookedSlot,


                },
                new ParkingTransaction()
                {
                    Id = Guid.NewGuid(),
                    ParkingZoneId = parkingZoneIdHaveBookedSlot,
                    CheckinAt = TimeHaveBookedSlot,
                    CheckoutAt = TimeHaveBookedSlot.AddHours(2),
                    Phone = phone,
                    Email = email,
                    LicensePlate = "29X3-12345",
                    StatusId = 1,
                    CreatedAt = TimeHaveBookedSlot,


                },
                new ParkingTransaction()
                {
                    Id = Guid.NewGuid(),
                    ParkingZoneId = parkingZoneIdHaveBookedSlot,
                    CheckinAt = TimeHaveBookedSlot,
                    CheckoutAt = TimeHaveBookedSlot.AddHours(2),
                    Phone = phone,
                    Email = email,
                    LicensePlate = "29X3-12345",
                    StatusId = 1,
                    CreatedAt = TimeHaveBookedSlot,


                },
                new ParkingTransaction()
                {
                    Id = Guid.NewGuid(),
                    ParkingZoneId = parkingZoneIdHaveBookedSlot,
                    CheckinAt = TimeHaveBookedSlot,
                    CheckoutAt = TimeHaveBookedSlot.AddHours(2),
                    Phone = phone,
                    Email = email,
                    LicensePlate = "29X3-12345",
                    StatusId = 1,
                    CreatedAt = TimeHaveBookedSlot,


                },
                new ParkingTransaction()
                {
                    Id = Guid.NewGuid(),
                    ParkingZoneId = parkingZoneIdHaveBookedSlot,
                    CheckinAt = TimeHaveBookedSlot,
                    CheckoutAt = TimeHaveBookedSlot.AddHours(2),
                    Phone = phone,
                    Email = email,
                    LicensePlate = "29X3-12345",
                    StatusId = 1,
                    CreatedAt = TimeHaveBookedSlot,


                },
            }).AsEnumerable();
        }
        IEnumerable<ParkingZone> GetParkingZoneData()
        {
            var city = FakeItEasy.A.Fake<City>();
            var district = FakeItEasy.A.Fake<District>();
            district.City = city;
            var commune = FakeItEasy.A.Fake<Commune>();
            commune.District = district;
            var owner = FakeItEasy.A.Fake<ParkingZoneOwner>();
            var parkingZone = FakeItEasy.A.Fake<ParkingZone>();
            parkingZone.Id = parkingZoneIdHaveBookedSlot;
            parkingZone.Owner = owner;
            parkingZone.Slots = 10;
            parkingZone.Commune = commune;
            parkingZone.DetailAddress = "asdf";
            parkingZone.Name = "parking zone mock";
            return (new List<ParkingZone>()
            {
parkingZone

            }).AsEnumerable();
        }
        public BookingTest(ITestOutputHelper output) : base(output)
        {
            var dbConfigOptions = new DbContextOptionsBuilder<FALL23_SWP490_G14Context>()
                .UseInMemoryDatabase(databaseName: "VPSContext")
                ;
            dbConfigOptions.ConfigureWarnings(d => d.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            var context = new FALL23_SWP490_G14Context(dbConfigOptions.Options);
            context.ParkingZones.AddRange(GetParkingZoneData());
            context.ParkingTransactions.AddRange(GetTestData());

            context.SaveChanges();
            var parkingTransactionRepo = new ParkingTransactionRepository(context);
            var parkingZoneRepo = new ParkingZoneRepository(context);

            parkingTransactionController = new Service.ManagerVPS.Controllers.ParkingTransactionController(
               parkingTransactionRepo,
                googleApiService,
                options,
               parkingZoneRepo,
                paymentTransactionRepository);
        }
        [Fact]
        public async Task Ok()
        {
            BookingSlot bookingSlot = new BookingSlot()
            {
                Email = "nghiaa@asdf.cn",
                Phone = "2131232123",
                CheckinAt = TimeHaveBookedSlot.AddHours(4),
                CheckoutAt = TimeHaveBookedSlot.AddHours(5),
                ParkingZoneId = parkingZoneIdHaveBookedSlot,
                LicensePlate = "29X3-12345"

            };
            var result = await parkingTransactionController.Booking(bookingSlot);

            output.WriteLine(result.ToString());
            Assert.NotNull(result);
        }
        [Fact]
        public async Task BookingDuplicateSameTime()
        {
            BookingSlot bookingSlot = new BookingSlot()
            {
                Email = "nghiaa@asdf.cn",
                Phone = "2131232123",
                CheckinAt = TimeHaveBookedSlot,
                CheckoutAt = TimeHaveBookedSlot.AddHours(5),
                ParkingZoneId = parkingZoneIdHaveBookedSlot,
                LicensePlate = "29X3-12345"

            };
            var ex = await Assert.ThrowsAsync<ClientException>(async () => await parkingTransactionController.Booking(bookingSlot));
            Assert.Equal(1003, ex.Code);
        }
        [Fact]
        public async Task ParkingZoneFullSlotAtCheckinTime()
        {
            BookingSlot bookingSlot = new BookingSlot()
            {
                Email = "nghiaa@asdf.cn",
                Phone = "2131232123",
                CheckinAt = TimeHaveBookedSlot,
                CheckoutAt = TimeHaveBookedSlot.AddHours(5),
                ParkingZoneId = parkingZoneIdHaveBookedSlot,
                LicensePlate = "29X3-324235"

            };
            var ex = await Assert.ThrowsAsync<ClientException>(async () => await parkingTransactionController.Booking(bookingSlot));
            Assert.Equal(1004, ex.Code);
        }
        [Fact]
        public async Task ParkingZoneFullSlotAtCheckoutTime()
        {
            BookingSlot bookingSlot = new BookingSlot()
            {
                Email = "nghiaa@asdf.cn",
                Phone = "2131232123",
                CheckinAt = TimeHaveBookedSlot.AddHours(-2),
                CheckoutAt = TimeHaveBookedSlot,
                ParkingZoneId = parkingZoneIdHaveBookedSlot,
                LicensePlate = "29X3-324235"

            };
            var ex = await Assert.ThrowsAsync<ClientException>(async () => await parkingTransactionController.Booking(bookingSlot));
            Assert.Equal(1005, ex.Code);
        }

    }
}
