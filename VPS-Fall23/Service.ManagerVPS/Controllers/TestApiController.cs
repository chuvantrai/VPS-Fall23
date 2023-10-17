using Microsoft.AspNetCore.Mvc;
using Service.ManagerVPS.Constants.Enums;
using Service.ManagerVPS.DTO.Input;
using Service.ManagerVPS.Repositories.Interfaces;
using Service.ManagerVPS.Controllers.Base;
using Service.ManagerVPS.DTO.OtherModels;
using Service.ManagerVPS.Extensions.ILogic;
using Service.ManagerVPS.FilterPermissions;
using Service.ManagerVPS.Models;

namespace Service.ManagerVPS.Controllers;

public class TestApiController : VpsController<Account>
{
    private readonly IConfiguration _config;
    private readonly IVnPayLibrary _vnPayLibrary;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TestApiController(IUserRepository userRepository, IConfiguration config,
        IVnPayLibrary vnPayLibrary, IHttpContextAccessor httpContextAccessor) : base(userRepository)
    {
        _config = config;
        _vnPayLibrary = vnPayLibrary;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpPost]
    [FilterPermission(Action = ActionFilterEnum.AddUser)]
    public IActionResult AddUser([FromForm] AddUserRequest request)
    {
        var t = ((IUserRepository)vpsRepository).AddUser();
        return Ok(t);
    }

    [HttpPost]
    // [FilterPermission(Action = ActionFilterEnum.TestAuthApi)]
    public IActionResult TestAuthApi()
    {
        return Ok("haha");
    }

    [HttpGet]
    // [FilterPermission(Action = ActionFilterEnum.CreatApiBankingDemo)]
    public IActionResult CreatApiBankingDemo()
    {
        var vnp_Returnurl = _config.GetValue<string>("vnpayConfig:vnp_Returnurl"); //URL nhan ket qua tra ve 
        var vnp_Url = _config.GetValue<string>("vnpayConfig:vnp_Url"); //URL thanh toan cua VNPAY 
        var vnp_TmnCode =
            _config.GetValue<string>("vnpayConfig:vnp_TmnCode"); //Ma định danh merchant kết nối (Terminal Id)
        var vnp_HashSecret = _config.GetValue<string>("vnpayConfig:vnp_HashSecret");

        //Get payment input
        var order = new OrderInfo
        {
            OrderId = DateTime.Now.Ticks, // Giả lập mã giao dịch hệ thống merchant gửi sang VNPAY
            Status = "0", //0: Trạng thái thanh toán "chờ thanh toán" hoặc "Pending" khởi tạo giao dịch chưa có IPN
            CreatedDate = DateTime.Now,
            Amount = 50000 // Giả lập số tiền thanh toán hệ thống merchant gửi sang VNPAY 50,000 VND
        };

        //Build URL for VNPAY
        _vnPayLibrary.AddRequestData("vnp_Version", _vnPayLibrary.GetVersionVnPay());
        _vnPayLibrary.AddRequestData("vnp_Command", "pay");
        _vnPayLibrary.AddRequestData("vnp_TmnCode", vnp_TmnCode);
        _vnPayLibrary.AddRequestData("vnp_Amount", (order.Amount * 100).ToString());
        _vnPayLibrary.AddRequestData("vnp_BankCode", "VNBANK");
        _vnPayLibrary.AddRequestData("vnp_CreateDate", order.CreatedDate.ToString("yyyyMMddHHmmss"));
        _vnPayLibrary.AddRequestData("vnp_CurrCode", "VND");
        _vnPayLibrary.AddRequestData("vnp_IpAddr", GetIpAddress() ?? "123456789");
        _vnPayLibrary.AddRequestData("vnp_Locale", "vn");
        _vnPayLibrary.AddRequestData("vnp_OrderInfo", "Thanh toan don hang:" + order.OrderId);
        _vnPayLibrary.AddRequestData("vnp_OrderType", "other"); //default value: other
        _vnPayLibrary.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
        _vnPayLibrary.AddRequestData("vnp_TxnRef", order.OrderId.ToString());
        var paymentUrl = _vnPayLibrary.CreateRequestUrl(vnp_Url, vnp_HashSecret);
        return Ok(new
        {
            PaymentUrl = paymentUrl,
            OrderId = order.OrderId, // id giao dịch
            VnpTmnCode = vnp_TmnCode
        });
    }

    private string? GetIpAddress()
    {
        string? ipAddress;
        try
        {
            ipAddress = _httpContextAccessor.HttpContext?.Request.Headers["X-Forwarded-For"];


            if (string.IsNullOrEmpty(ipAddress) || (ipAddress.ToLower() == "unknown") || ipAddress.Length > 45)
                ipAddress = _httpContextAccessor.HttpContext?.Request.Headers["REMOTE_ADDR"];
        }
        catch (Exception ex)
        {
            ipAddress = "Invalid IP:" + ex.Message;
        }

        return ipAddress;
    }
}