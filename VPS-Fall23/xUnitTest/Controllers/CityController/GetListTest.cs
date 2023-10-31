using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Service.ManagerVPS.Models;
using Service.ManagerVPS.Repositories;

namespace xUnitTest.Controllers.CityController
{
    public class GetListTest
    {
        readonly Service.ManagerVPS.Controllers.CityController controller;
        readonly CityRepository cityRepository;
        public GetListTest()
        {

            var inMemoryDbOption = new DbContextOptionsBuilder<FALL23_SWP490_G14Context>()
               .UseInMemoryDatabase(databaseName: "VPSContext");
            inMemoryDbOption.ConfigureWarnings(d => d.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            FALL23_SWP490_G14Context fALL23_SWP490_G14Context = new FALL23_SWP490_G14Context(inMemoryDbOption.Options);

            cityRepository = new CityRepository(fALL23_SWP490_G14Context);
            controller = new Service.ManagerVPS.Controllers.CityController(cityRepository);
        }
        [Fact]
        public  void ResponseOk()
        {
       
            Assert.IsAssignableFrom<IEnumerable<City>>(controller.GetList());
        }
        [Fact]
        public async Task ResponseHasData()
        {
            var city = FakeItEasy.A.Fake<City>();
            city.Id = Guid.NewGuid();
            city.Name = "city abasdf";
            city.Code = 12321;
            await cityRepository.Create(city);
            await cityRepository.SaveChange();
            Assert.NotEmpty(controller.GetList());
        }


    }
}
