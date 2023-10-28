using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Options;
using Service.ManagerVPS.Constants.Notifications;
using Service.ManagerVPS.DTO.Exceptions;
using Service.ManagerVPS.DTO.VNPay;
using Service.ManagerVPS.ExternalClients;
using Service.ManagerVPS.Models;
using Service.ManagerVPS.Repositories;
using Service.ManagerVPS.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace xUnitTest.Controllers.ParkingTransactionController
{
    public class GetPayUrl : GlobalBase
    {
        readonly Service.ManagerVPS.Controllers.ParkingTransactionController parkingTransactionController;
        readonly GoogleApiService googleApiService = FakeItEasy.A.Fake<GoogleApiService>();
        readonly IOptions<VnPayConfig> options;
        readonly IPaymentTransactionRepository paymentTransactionRepository;
        readonly IParkingTransactionRepository parkingTransactionRepository;
        readonly IParkingZoneRepository parkingZoneRepository;
        Guid parkingTransactionid = Guid.NewGuid();
        List<ParkingTransaction> GetParkingTransactionsTest()
        {
            var parkingZone = FakeItEasy.A.Fake<ParkingZone>();
            parkingZone.PricePerHour = 100000;
            parkingZone.Id = Guid.NewGuid();
            parkingZone.DetailAddress = "asdf";
            parkingZone.Name = "parking zone mock";
            var transaction = FakeItEasy.A.Fake<ParkingTransaction>();
            transaction.Id = parkingTransactionid;
            transaction.LicensePlate = "29X3-232333";
            var checkinAt = DateTime.Now;
            transaction.CheckinAt = checkinAt;
            transaction.CheckoutAt = checkinAt.AddHours(2);
            transaction.StatusId = 1;
            transaction.Email = "sdfasdf@gmail.com";
            transaction.Phone = "3423432324";
            transaction.ParkingZone = parkingZone;
            transaction.ParkingZoneId = parkingZone.Id;

            return new List<ParkingTransaction>()
            {
                transaction
            };
        }
        public GetPayUrl(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            VnPayConfig vnPayConfig = new VnPayConfig();
            using (FileStream file = new FileStream("appsettings.json", FileMode.Open, FileAccess.Read))
            {
                vnPayConfig = JsonObject.Parse(file)["vnPay"].Deserialize<VnPayConfig>();
            };
            options = Options.Create<VnPayConfig>(vnPayConfig);
            var dbConfigOptions = new DbContextOptionsBuilder<FALL23_SWP490_G14Context>()
               .UseInMemoryDatabase(databaseName: "VPSContext")
               ;
            dbConfigOptions.ConfigureWarnings(d => d.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            var context = new FALL23_SWP490_G14Context(dbConfigOptions.Options);
            var parkingTransaction = GetParkingTransactionsTest();
            context.ParkingZones.Add(parkingTransaction.First().ParkingZone);
            context.ParkingTransactions.AddRange(parkingTransaction);

            context.SaveChanges();
            parkingTransactionRepository = new ParkingTransactionRepository(context);
            parkingZoneRepository = new ParkingZoneRepository(context);
            paymentTransactionRepository = new PaymentTransactionRepository(context);
            parkingTransactionController =
                new Service.ManagerVPS.Controllers.ParkingTransactionController(
                    parkingTransactionRepository,
                googleApiService,
                options,
                parkingZoneRepository,
                paymentTransactionRepository);
        }
        [Fact]
        public async Task GetPayUrlOk()
        {
            var id = parkingTransactionid;
            var result = await parkingTransactionController.GetPayUrl(id);
            Assert.NotNull(result);
        }
        [Fact]
        public async Task CreatedNewPaymentTransaction()
        {
            var id = parkingTransactionid;
            var result = await parkingTransactionController.GetPayUrl(id);
            var paymentTransaction = await paymentTransactionRepository.Entities.FirstOrDefaultAsync(p => p.BookingId == id);
            output.WriteLine(paymentTransaction.Amount.ToString());
            Assert.NotNull(result);

        }
        [Fact]
        public async Task RightAmount()
        {
            var id = parkingTransactionid;
            var result = await parkingTransactionController.GetPayUrl(id);
            var paymentTransaction = await paymentTransactionRepository.Entities.FirstOrDefaultAsync(p => p.BookingId == id);
            var parkingTransaction = await parkingTransactionRepository.Find(id);
            var parkingZone = await parkingZoneRepository.Find(parkingTransaction.ParkingZoneId);

            Assert.Equal(paymentTransaction.Amount, parkingZone.PricePerHour * (parkingTransaction.CheckoutAt - parkingTransaction.CheckinAt).Value.Hours);
        }
        [Fact]
        public async Task NotExistTransaction()
        {
            var id = Guid.NewGuid();

            var exception = await Assert.ThrowsAsync<ClientException>(async () => await parkingTransactionController.GetPayUrl(id));
            Assert.Equal(ResponseNotification.NOT_FOUND, exception.Message);
        }
    }
}
