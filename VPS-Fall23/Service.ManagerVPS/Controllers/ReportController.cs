using Microsoft.AspNetCore.Mvc;
using Service.ManagerVPS.Constants.Enums;
using Service.ManagerVPS.Constants.KeyValue;
using Service.ManagerVPS.Controllers.Base;
using Service.ManagerVPS.DTO.Exceptions;
using Service.ManagerVPS.DTO.Input;
using Service.ManagerVPS.DTO.OtherModels;
using Service.ManagerVPS.Extensions.ILogic;
using Service.ManagerVPS.Extensions.StaticLogic;
using Service.ManagerVPS.FilterPermissions;
using Service.ManagerVPS.Models;
using Service.ManagerVPS.Repositories.Interfaces;

namespace Service.ManagerVPS.Controllers;

public class ReportController : VpsController<Report>
{
    private readonly IGeneralVPS _generalVps;

    public ReportController(IReportRepository reportRepository, IGeneralVPS generalVps) : base(reportRepository)
    {
        _generalVps = generalVps;
    }

    [HttpPost]
    [FilterPermission(Action = ActionFilterEnum.CreateReport)]
    public async Task<IActionResult> CreateReport(CreateReportRequest request)
    {
        var accessToken = Request.Cookies["ACCESS_TOKEN"];
        if (accessToken != null)
        {
            var userToken = JwtTokenExtension.ReadToken(accessToken)!;
            request.UserId = Guid.Parse(userToken.UserId);
        }

        if ((request.UserId is null && request.Email == null && request.Phone == null)
            || (string.IsNullOrEmpty(request.PaymentCode) && request.Type == ReportTypeEnum.REQUEST_TRANSACTION_REFUND))
        {
            throw new ClientException();
        }

        if (request.Type is ReportTypeEnum.REQUEST_TRANSACTION_REFUND or ReportTypeEnum.TRANSACTION_ERROR)
        {
            var checkPaymentCode = await ((IReportRepository)vpsRepository)
                .CheckPaymentCodeInReport(request.PaymentCode!, (int)request.Type);
            if (checkPaymentCode != null)
            {
                throw new ClientException((int)checkPaymentCode);
            }
        }

        var report = await ((IReportRepository)vpsRepository).CreateReport(request);

        // send mail to admin about report
        // Content email
        var keyValuesTemplate = new List<KeyValue>
        {
            new KeyValue
            {
                Key = KeyHtmlEmail.TITLE_CONTENT,
                Value = report.TypeNavigation.Description
            },
            new KeyValue
            {
                Key = KeyHtmlEmail.DATE_SEND,
                Value = report.CreatedAt.ToString("dd-MM-yyyy hh:mm:ss")
            },
            new KeyValue
            {
                Key = KeyHtmlEmail.TYPE_REPORT,
                Value = report.TypeNavigation.Description
            },
            new KeyValue
            {
                Key = KeyHtmlEmail.USER_SEND,
                Value = report.CreatedByNavigation != null
                    ? report.CreatedByNavigation.FirstName + " " + report.CreatedByNavigation.LastName
                    : string.Empty
            },
            new KeyValue
            {
                Key = KeyHtmlEmail.EMAIL_USER,
                Value = report.CreatedByNavigation != null ? report.CreatedByNavigation.Email : report.Email
            },
            new KeyValue
            {
                Key = KeyHtmlEmail.PHONE_USER,
                Value = report.CreatedByNavigation != null ? report.CreatedByNavigation.PhoneNumber : report.Phone
            },
            new KeyValue
            {
                Key = KeyHtmlEmail.DESCRIPTRION,
                Value = report.Content
            },
            new KeyValue()
            {
                Key = KeyHtmlEmail.CSS_PAYMENTCODE,
                Value = request.Type == ReportTypeEnum.REQUEST_TRANSACTION_REFUND ? "" : "display: none;"
            }
        };

        if (request.Type == ReportTypeEnum.REQUEST_TRANSACTION_REFUND)
        {
            keyValuesTemplate.Add(new KeyValue()
            {
                Key = KeyHtmlEmail.PAYMENTCODE,
                Value = report.PaymentCode ?? ""
            });
        }

        var templateEmail = _generalVps.CreateTemplateEmail(keyValuesTemplate);
        var toEmail = new[] { "traicvhe153014@fpt.edu.vn", "0362351671trai@gmail.com" };
        var titleEmail = $"Thông báo người dùng gửi báo cáo về {report.TypeNavigation.Description}";
        await _generalVps.SendListEmailAsync(toEmail, titleEmail, templateEmail);

        return Ok();
    }

    [HttpGet]
    [FilterPermission(Action = ActionFilterEnum.GetReportForAdmin)]
    public IActionResult GetReportForAdmin([FromQuery] QueryStringParameters parameters)
    {
        var listReport =
            ((IReportRepository)vpsRepository).GetListReportForAdmin(parameters);

        var result = listReport
            .Select((x, index) => new
            {
                Key = index + 1,
                x.SubId,
                x.Id,
                TypeName = x.TypeNavigation.Name,
                x.Email,
                CreatedAt = $"{x.CreatedAt:dd-MM-yyyy}",
                x.Content,
                Status = x.StatusNavigation.Name,
            }).ToList();

        var metadata = new
        {
            listReport.TotalCount,
            listReport.PageSize,
            listReport.CurrentPage,
            listReport.TotalPages,
            listReport.HasNext,
            listReport.HasPrev,
            Data = result
        };
        return Ok(metadata);
    }

    [HttpGet]
    [FilterPermission(Action = ActionFilterEnum.GetTypeReport)]
    public IActionResult GetTypeReport()
    {
        var listType =
            ((IReportRepository)vpsRepository).GetTypeReport();
        var result = listType.Select(lt => new { Value = lt.Id, Label = lt.Name }).ToList();

        return Ok(result);
    }

    [HttpGet]
    [FilterPermission(Action = ActionFilterEnum.FilterReport)]
    public IActionResult FilterReport([FromQuery] QueryStringParameters parameters, [FromQuery] int typeId)
    {

        var listReport =
            ((IReportRepository)vpsRepository).FilterReportForAdmin(parameters, typeId);

        var result = listReport
            .Select((x, index) => new
            {
                Key = index + 1,
                x.SubId,
                x.Id,
                TypeName = x.TypeNavigation.Name,
                x.Email,
                CreatedAt = $"{x.CreatedAt:dd-MM-yyyy}",
                x.Content,
                Status = x.StatusNavigation.Name,
            }).ToList();

        var metadata = new
        {
            listReport.TotalCount,
            listReport.PageSize,
            listReport.CurrentPage,
            listReport.TotalPages,
            listReport.HasNext,
            listReport.HasPrev,
            Data = result
        };
        return Ok(metadata);
    }

    [HttpPost]
    [FilterPermission(Action = ActionFilterEnum.UpdateStatusReport)]
    public IActionResult UpdateStatusReport([FromQuery] Guid reportId, [FromQuery] int statusId)
    {
        ((IReportRepository)vpsRepository).UpdateStatusReportAsync(reportId, statusId);

        return Ok();
    }
}