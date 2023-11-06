using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Service.ManagerVPS.Controllers;
using Service.ManagerVPS.DTO.AppSetting;
using Service.ManagerVPS.DTO.Input;
using Service.ManagerVPS.Extensions.ILogic;
using Service.ManagerVPS.Models;
using Service.ManagerVPS.Repositories.Interfaces;
using Xunit.Abstractions;

namespace xUnitTest.Controllers;

public class AuthControllerTests : GlobalBase
{
    private readonly IUserRepository _userRepository = A.Fake<IUserRepository>();
    private readonly IGeneralVPS _generalVps = A.Fake<IGeneralVPS>();
    private readonly IParkingZoneOwnerRepository _ownerRepository = A.Fake<IParkingZoneOwnerRepository>();
    private readonly IConfiguration _config = A.Fake<IConfiguration>();
    private readonly IOptions<FileManagementConfig> options = A.Fake<IOptions<FileManagementConfig>>();

    public AuthControllerTests(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void AuthController_Register_ReturnOK()
    {
        var verifyCode = 123456;
        var input = A.Fake<RegisterAccount>();
        var existedAccount = A.Fake<Account>();
        var parkingZoneOwnerExistedAccount = existedAccount.ParkingZoneOwner!;
        var newAccount = A.Fake<Account>();
        var parkingZoneOwner = A.Fake<ParkingZoneOwner>();

        A.CallTo(() => _userRepository.GetOwnerAccountByEmail(input.Email)).Returns(existedAccount);
        A.CallTo(() => _generalVps.GenerateVerificationCode()).Returns(verifyCode);
        A.CallTo(() => _userRepository.Update(existedAccount));
        A.CallTo(() => _ownerRepository.Update(parkingZoneOwnerExistedAccount));
        A.CallTo(() => _userRepository.Create(newAccount));
        A.CallTo(() => _ownerRepository.Create(parkingZoneOwner));

        var controller = new AuthController(_userRepository, _generalVps, _ownerRepository, _config, options);
        base.output.WriteLine(existedAccount.Id.ToString());
        var result = controller.Register(input);

        result.Should().BeOfType(typeof(Task<IActionResult>));
    }
}