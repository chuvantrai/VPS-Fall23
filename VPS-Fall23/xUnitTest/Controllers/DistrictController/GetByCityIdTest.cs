using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Service.ManagerVPS.Models;
using Service.ManagerVPS.Repositories;

namespace xUnitTest.Controllers.DistrictController
{
    public class GetByCityIdTest
    {
        readonly Service.ManagerVPS.Controllers.DistrictController controller;
        readonly DistrictRepository districtRepo;
        public GetByCityIdTest()
        {

            var inMemoryDbOption = new DbContextOptionsBuilder<FALL23_SWP490_G14Context>()
               .UseInMemoryDatabase(databaseName: "VPSContext");
            inMemoryDbOption.ConfigureWarnings(d => d.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            FALL23_SWP490_G14Context fALL23_SWP490_G14Context = new FALL23_SWP490_G14Context(inMemoryDbOption.Options);

            districtRepo = new DistrictRepository(fALL23_SWP490_G14Context);
            controller = new Service.ManagerVPS.Controllers.DistrictController(districtRepo);
        }

        [Fact]
        public async Task ResponseOk()
        {
            var result = await controller.GetDistrictByCityId(Guid.NewGuid());
            Assert.IsAssignableFrom<IEnumerable<District>>(result);
        }
        [Fact]
        public async Task ResponseShouldEmpty()
        {
            var result = await controller.GetDistrictByCityId(Guid.NewGuid());
            Assert.Empty(result);
        }
        [Fact]
        public async Task ResponseShouldHaveData()
        {
            Guid cityId = Guid.NewGuid();
            var district = new District()
            {
                Id = Guid.NewGuid(),
                Code = 123,
                Name = "Commune Test abc",
                CityId = cityId,
                City = new City()
                {
                    Id = cityId,
                    Name = "district testabcd",
                    Code = 123213
                }
            };
            await districtRepo.Create(district);
            await districtRepo.SaveChange();
            Assert.NotEmpty(await controller.GetDistrictByCityId(cityId));
        }
    }
}
