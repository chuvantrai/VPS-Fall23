using Microsoft.AspNetCore.Mvc;
using Service.ManagerVPS.Constants.Enums;
using Service.ManagerVPS.Constants.Notifications;
using Service.ManagerVPS.FilterPermissions;
using Service.ManagerVPS.DTO.Input;
using Service.ManagerVPS.Repositories.Interfaces;

namespace Service.ManagerVPS.Controllers;

[ApiController]
[Route("[controller]")]
public class TestApiController : Controller
{
    private readonly IUserRepository _userRepository;

    public TestApiController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpGet("GetBadRequest")]
    public IActionResult GetBadRequest()
    {
        return BadRequest(ResponseNotification.SERVER_ERROR);
    }

    [HttpGet("GetOk")]
    public IActionResult GetOk()
    {
        return Ok(ResponseNotification.ADD_SUCCESS);
    }

    [HttpPost("AddUser")]
    [FilterPermission(Action = ActionFilterEnum.AddUser)]
    public IActionResult AddUser([FromForm] AddUserRequest request)
    {
        var t = _userRepository.AddUser();
        return Ok(t);
    }
    
    [HttpPost("TestAuthApi")]
    [FilterPermission(Action = ActionFilterEnum.TestAuthApi)]
    public IActionResult TestAuthApi()
    {
        return Ok("haha");
    }

}