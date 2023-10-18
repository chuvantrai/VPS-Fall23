using System.Net;
using Microsoft.AspNetCore.Mvc;
using Service.ManagerVPS.Constants.Enums;
using Service.ManagerVPS.DTO.Input;
using Service.ManagerVPS.Repositories.Interfaces;
using Service.ManagerVPS.Controllers.Base;
using Service.ManagerVPS.DTO.OtherModels;
using Service.ManagerVPS.Extensions.ILogic;
using Service.ManagerVPS.Extensions.Logic;
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
        _vnPayLibrary.AddRequestData("vnp_OrderInfo", "Thanh toan don hang: " + order.OrderId);
        _vnPayLibrary.AddRequestData("vnp_OrderType", "other"); //default value: other
        _vnPayLibrary.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
        _vnPayLibrary.AddRequestData("vnp_TxnRef", order.OrderId.ToString());
        var paymentUrl = _vnPayLibrary.CreateRequestUrl(vnp_Url, vnp_HashSecret);
        var orderId = order.OrderId; // mã giao dịch
        return Ok(new
        {
            PaymentUrl = paymentUrl,
            vnp_TxnRef = orderId.ToString(),
            vnp_CreateDate = order.CreatedDate.ToString("yyyyMMddHHmmss")
        });
    }

    [HttpPost]
    public IActionResult QueryDrVnPayDemo(string vnpTxnRef, string vnpCreateDate)
    {
        var vnp_Api = _config.GetValue<string>("vnpayConfig:vnp_Api");
        var vnp_HashSecret = _config.GetValue<string>("vnpayConfig:vnp_HashSecret"); //Secret KEy
        var vnp_TmnCode = _config.GetValue<string>("vnpayConfig:vnp_TmnCode"); // Terminal Id

        //Mã hệ thống merchant tự sinh ứng với mỗi yêu cầu truy vấn giao dịch.
        //Mã này là duy nhất dùng để phân biệt các yêu cầu truy vấn giao dịch. Không được trùng lặp trong ngày.
        var vnp_RequestId = DateTime.Now.Ticks.ToString(); 
        var vnp_Version = _vnPayLibrary.GetVersionVnPay(); //2.1.0
        var vnp_Command = "querydr";
        var vnp_TxnRef = vnpTxnRef; // Mã giao dịch thanh toán tham chiếu
        var vnp_OrderInfo = "Truy van giao dich:" + vnpTxnRef;
        var vnp_TransactionDate = vnpCreateDate;
        var vnp_CreateDate = DateTime.Now.ToString("yyyyMMddHHmmss");
        var vnp_IpAddr = GetIpAddress();

        var signData = vnp_RequestId + "|" + vnp_Version + "|" + vnp_Command + "|" + vnp_TmnCode + "|" + vnp_TxnRef +
                       "|" + vnp_TransactionDate + "|" + vnp_CreateDate + "|" + vnp_IpAddr + "|" + vnp_OrderInfo;
        var vnp_SecureHash = Utils.HmacSHA512(vnp_HashSecret, signData);

        var qdrData = new
        {
            vnp_RequestId = vnp_RequestId,
            vnp_Version = vnp_Version,
            vnp_Command = vnp_Command,
            vnp_TmnCode = vnp_TmnCode,
            vnp_TxnRef = vnp_TxnRef,
            vnp_OrderInfo = vnp_OrderInfo,
            vnp_TransactionDate = vnp_TransactionDate,
            vnp_CreateDate = vnp_CreateDate,
            vnp_IpAddr = vnp_IpAddr,
            vnp_SecureHash = vnp_SecureHash
        };
        var jsonData = System.Text.Json.JsonSerializer.Serialize(qdrData);

        var httpWebRequest = (HttpWebRequest)WebRequest.Create(vnp_Api);
        httpWebRequest.ContentType = "application/json";
        httpWebRequest.Method = "POST";

        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
        {
            streamWriter.Write(jsonData);
        }

        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
        var strData = "";
        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        {
            strData = streamReader.ReadToEnd();
        }

        return Ok(strData);
    }

    private string GetIpAddress()
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

        return ipAddress??"::1";
    }
}