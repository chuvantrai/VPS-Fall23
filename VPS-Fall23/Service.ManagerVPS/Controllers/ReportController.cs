using Microsoft.AspNetCore.Mvc;
using Service.ManagerVPS.Constants.Enums;
using Service.ManagerVPS.Constants.KeyValue;
using Service.ManagerVPS.Controllers.Base;
using Service.ManagerVPS.DTO.Input;
using Service.ManagerVPS.DTO.OtherModels;
using Service.ManagerVPS.Extensions.ILogic;
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
    public async Task<IActionResult> CreateReport([FromForm]CreateReportRequest request)
    {
        var report = await ((IReportRepository)vpsRepository).CreateReport(request);

        // send mail to admin about report
        // Content email
        var templateEmail = _generalVps.CreateTemplateEmail(new List<KeyValue>
        {
            new KeyValue
            {
                Key = KeyHtmlEmail.TITLE_CONTENT,
                Value = report.TypeNavigation.Description
            },
            new KeyValue
            {
                Key = KeyHtmlEmail.DATE_SEND,
                Value = report.CreatedAt.ToString("dd-MM-YYYY hh:mm:ss")
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
                    ? report.CreatedByNavigation.FirstName + " " + report.CreatedByNavigation.FirstName
                    : string.Empty
            },
            new KeyValue
            {
                Key = KeyHtmlEmail.EMAIL_USER,
                Value = report.CreatedByNavigation != null ? report.CreatedByNavigation.Email : request.Email
            },
            new KeyValue
            {
                Key = KeyHtmlEmail.PHONE_USER,
                Value = report.CreatedByNavigation != null ? report.CreatedByNavigation.PhoneNumber : request.Phone
            },
            new KeyValue
            {
                Key = KeyHtmlEmail.DESCRIPTRION,
                Value = report.Content
            }
        });
        var toEmail = new[] { "traicvhe153014@fpt.edu.vn", "0362351671trai@gmail.com" };

        // await _generalVps.SendListEmailAsync(toEmail, titleEmail, templateEmail);
        return Ok(templateEmail);
    }
}