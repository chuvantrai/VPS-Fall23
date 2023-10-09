using Microsoft.AspNetCore.Mvc;
using Service.ManagerVPS.Constants.Enums;
using Service.ManagerVPS.DTO.Input;
using Service.ManagerVPS.Repositories.Interfaces;
using Service.ManagerVPS.Controllers.Base;
using Service.ManagerVPS.FilterPermissions;
using Service.ManagerVPS.Models;

namespace Service.ManagerVPS.Controllers;

public class TestApiController : VpsController<Account>
{

    public TestApiController(IUserRepository userRepository) : base(userRepository)
    {
    }

    [HttpPost("AddUser")]
    [FilterPermission(Action = ActionFilterEnum.AddUser)]
    public IActionResult AddUser([FromForm] AddUserRequest request)
    {
        var t = ((IUserRepository)vpsRepository).AddUser();
        return Ok(t);
    }
    
    [HttpPost("TestAuthApi")]
    [FilterPermission(Action = ActionFilterEnum.TestAuthApi)]
    public IActionResult TestAuthApi()
    {
        return Ok("haha");
    }
}
