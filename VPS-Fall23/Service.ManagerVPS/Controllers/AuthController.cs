using Microsoft.AspNetCore.Mvc;
using Service.ManagerVPS.DTO.Input;
using Service.ManagerVPS.Models;

namespace Service.ManagerVPS.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : Controller
{
    private readonly FALL23_SWP490_G14Context _context;

    public AuthController(FALL23_SWP490_G14Context context)
    {
        _context = context;
    }
    
    [HttpPost("AuthLogin")]
    public async Task<IActionResult> AuthLogin()
    {
        return Ok();
    }
    
    [HttpPost("CreateAccountDemo")]
    public async Task<IActionResult> CreateAccountDemo([FromForm] CreateAccountDemoRegister request)
    {
        var newAccount = new Account()
        {
            TypeId = (int)request.TypeId,
        };
        _context.Accounts.Add(newAccount);
        await _context.SaveChangesAsync();
        return Ok();
    }
}