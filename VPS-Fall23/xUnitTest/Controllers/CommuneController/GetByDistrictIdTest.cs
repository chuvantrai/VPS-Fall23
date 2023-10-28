using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Service.ManagerVPS.Models;
using Service.ManagerVPS.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xUnitTest.Controllers.CommuneController
{
    public class GetByDistrictIdTest
    {
        readonly Service.ManagerVPS.Controllers.CommuneController controller;
        readonly CommuneRepository comuneRepo;
        public GetByDistrictIdTest()
        {

            var inMemoryDbOption = new DbContextOptionsBuilder<FALL23_SWP490_G14Context>()
               .UseInMemoryDatabase(databaseName: "VPSContext");
            inMemoryDbOption.ConfigureWarnings(d => d.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            FALL23_SWP490_G14Context fALL23_SWP490_G14Context = new FALL23_SWP490_G14Context(inMemoryDbOption.Options);

            comuneRepo = new CommuneRepository(fALL23_SWP490_G14Context);
            controller = new Service.ManagerVPS.Controllers.CommuneController(comuneRepo);
        }

        [Fact]
        public async Task ResponseOk()
        {
            var result = await controller.GetCommuneByDistrict(Guid.NewGuid());
            Assert.IsAssignableFrom<IEnumerable<Commune>>(result);
        }
        [Fact]
        public async Task ResponseShouldEmpty()
        {
            var result = await controller.GetCommuneByDistrict(Guid.NewGuid());
            Assert.Empty(result);
        }
        [Fact]
        public async Task ResponseShouldHaveData()
        {
            Guid districtId = Guid.NewGuid();
            var commune = new Commune()
            {
                Id = Guid.NewGuid(),
                Code = 123,
                Name = "Commune Test abc",
                DistrictId = districtId,
                District = new District()
                {
                    Id = districtId,
                    Name = "district testabcd",
                    Code = 123213
                }
            };
            await comuneRepo.Create(commune);
            await comuneRepo.SaveChange();
            Assert.NotEmpty(await controller.GetCommuneByDistrict(districtId));
        }

    }
}
