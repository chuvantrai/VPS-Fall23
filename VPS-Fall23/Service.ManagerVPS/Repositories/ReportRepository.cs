using Microsoft.EntityFrameworkCore;
using Service.ManagerVPS.Constants.Enums;
using Service.ManagerVPS.DTO.Input;
using Service.ManagerVPS.DTO.OtherModels;
using Service.ManagerVPS.Models;
using Service.ManagerVPS.Repositories.Interfaces;
using System.Net.WebSockets;

namespace Service.ManagerVPS.Repositories;

public class ReportRepository : VpsRepository<Report>, IReportRepository
{
    public ReportRepository(FALL23_SWP490_G14Context context) : base(context)
    {
    }

    public async Task<Report> CreateReport(CreateReportRequest request)
    {
        request.Email ??= string.Empty;
        request.Phone ??= string.Empty;

        var report = new Report
        {
            Id = Guid.NewGuid(),
            Content = request.Content,
            CreatedAt = DateTime.Now,
            Status = (int)ReportStatusEnum.PROCESSING,
            Type = (int)request.Type,
            CreatedBy = request.UserId,
            Email = request.Email,
            Phone = request.Phone,
            PaymentCode = request.PaymentCode
        };
        context.Reports.Add(report);
        await context.SaveChangesAsync();
        var reportResult = await context.Reports
            .Include(x => x.CreatedByNavigation)
            .Include(x => x.TypeNavigation)
            .Include(x => x.StatusNavigation)
            .FirstOrDefaultAsync(x => x.Id.Equals(report.Id));
        return reportResult!;
    }

    public PagedList<Report> GetListReportForAdmin(QueryStringParameters parameters)
    {
        var listReport = entities
             .Include(r => r.StatusNavigation)
             .Include(r => r.TypeNavigation)
             .Include(r => r.CreatedByNavigation)
             .Where(r => r.TypeNavigation.Name.Contains("ERROR"))
             .OrderBy(r => r.SubId);

        return PagedList<Report>.ToPagedList(listReport, parameters.PageNumber,
            parameters.PageSize);
    }

    public List<GlobalStatus> GetTypeReport()
    {
        var result = context.GlobalStatuses.Where(gs => gs.Name.Contains("ERROR")).ToList();
        return result;
    }

    public PagedList<Report> FilterReportForAdmin(QueryStringParameters parameters, int typeId)
    {
        var listReport = entities
             .Include(r => r.StatusNavigation)
             .Include(r => r.TypeNavigation)
             .Include(r => r.CreatedByNavigation)
             .Where(r => r.Type == typeId)
             .OrderBy(r => r.SubId);

        return PagedList<Report>.ToPagedList(listReport, parameters.PageNumber,
            parameters.PageSize);
    }

    public async Task UpdateStatusReportAsync(Guid reportId, int statusId)
    {
        var report = entities.Include(r => r.StatusNavigation).FirstOrDefault(r => r.Id == reportId);
        if (report != null)
        {
            report.Status = statusId;
            context.Reports.Update(report);
            await context.SaveChangesAsync();
        }       
    }

    public async Task<int?> CheckPaymentCodeInReport(string paymentCode, int type)
    {
        var reportResult = await context.Reports
            .FirstOrDefaultAsync(x => x.PaymentCode != null && x.PaymentCode.Equals(paymentCode) && x.Type == type);
        if (reportResult != null)
        {
            switch (type)
            {
                case (int)ReportTypeEnum.REQUEST_TRANSACTION_REFUND:
                    return 5012;
                case (int)ReportTypeEnum.TRANSACTION_ERROR:
                    return 5013;
            }
        }

        var payment = await context.PaymentTransactions
            .FirstOrDefaultAsync(x => x.TxnRef.Equals(paymentCode));
        if (payment == null)
        {
            return 5011;
        }

        return null;
    }
}