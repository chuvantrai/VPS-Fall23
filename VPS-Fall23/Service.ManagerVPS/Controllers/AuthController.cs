using System.ComponentModel.DataAnnotations;
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
    public async Task<IActionResult> AuthLogin(LoginRequest request)
    {
        if (request.Username == "123")
        {
            return BadRequest("213");
        }
        return Ok();
    }
    
    [HttpPost("CreateAccountDemo")]
    public async Task<IActionResult> CreateAccountDemo([FromForm] CreateAccountDemoRequest request)
    {
        var newAccount = new Account()
        {
            TypeId = (int)request.TypeId,
            Id = Guid.NewGuid(),
            Username = request.Username,
            Password = request.Password,
            FirstName = request.FirstName,
            LastName = request.LastName,
            CreatedAt = DateTime.Now,
            ModifiedAt = DateTime.Now,
            IsBlock = false,
            PhoneNumber = request.PhoneNumber
        };
        // _context.Accounts.Add(newAccount);
        // await _context.SaveChangesAsync();
        return Ok();
    }
}