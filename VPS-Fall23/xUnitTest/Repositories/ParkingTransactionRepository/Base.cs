using FakeItEasy;
using Xunit.Abstractions;
using Service.ManagerVPS.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Net.Sockets;

namespace xUnitTest.Repositories.ParkingTransactionRepository
{
    public class Base : GlobalBase
    {    // Arrange
        protected Guid parkingZoneIdHaveBookedSlot = Guid.NewGuid();
        protected Guid parkingZoneIdNotHaveBookedSlot = Guid.NewGuid();
        protected DateTime TimeHaveBookedSlot = DateTime.Now;
        protected DateTime TimeDoNotHaveBookedSlot = DateTime.Now.AddDays(-1);
        protected DateTime checkAt = DateTime.Now;
        protected Service.ManagerVPS.Repositories.ParkingTransactionRepository repository;

        public Base(ITestOutputHelper output) : base(output)
        {
            var options = new DbContextOptionsBuilder<FALL23_SWP490_G14Context>()
            .UseInMemoryDatabase(databaseName: "VPSContext")
            .Options;
            var context = new FALL23_SWP490_G14Context(options);
            context.ParkingTransactions.AddRange(GetTestData());
            context.SaveChanges();
            repository = new Service.ManagerVPS.Repositories.ParkingTransactionRepository(context);
        }
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
    }
}
