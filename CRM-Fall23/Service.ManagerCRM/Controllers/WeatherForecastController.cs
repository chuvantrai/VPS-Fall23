using Microsoft.AspNetCore.Mvc;
using Service.ManagerCRM.Constants.Enums;
using Service.ManagerCRM.Constants.Notifications;
using Service.ManagerCRM.DAL;
using Service.ManagerCRM.FilterPermissions;
using Service.ManagerCRM.Models;
using Service.ManagerCRM.ModelsNoneDB.RequestModels;

namespace Service.ManagerCRM.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet("GetTest")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
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
    public IActionResult AddUser([FromForm]AddUserRequest request)
    {
        var dal = new UserDal();
        var t = dal.AddUser();
        return Ok(t);
    }
}