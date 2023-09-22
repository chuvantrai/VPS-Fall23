using Microsoft.AspNetCore.Mvc;
using Service.ManagerVPS.Constants.Enums;
using Service.ManagerVPS.Constants.Notifications;
using Service.ManagerVPS.DAL;
using Service.ManagerVPS.FilterPermissions;
using Service.ManagerVPS.ModelsNoneDB.RequestModels;

namespace Service.ManagerVPS.Controllers;

[ApiController]
[Route("[controller]")]
public class TestApiController : Controller
{
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
    public IActionResult AddUser([FromForm]AddUserRequest request)
    {
        var dal = new UserDal();
        var t = dal.AddUser();
        return Ok(t);
    }
}