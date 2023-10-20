using Microsoft.EntityFrameworkCore;
using Service.ManagerVPS.Constants.Enums;
using Service.ManagerVPS.DTO.Input;
using Service.ManagerVPS.Models;
using Service.ManagerVPS.Repositories.Interfaces;

namespace Service.ManagerVPS.Repositories;

public class ReportRepository : VpsRepository<Report>, IReportRepository
{
    public ReportRepository(FALL23_SWP490_G14Context context) : base(context)
    {
    }

    public async Task<Report> CreateReport(CreateReportRequest request)
    {
        var report = new Report
        {
            Id = Guid.NewGuid(),
            Content = request.Content,
            CreatedAt = DateTime.Now,
            Status = (int)ReportStatusEnum.PROCESSING,
            Type = (int)request.Type,
            CreatedBy = request.UserId
        };
        context.Reports.Add(report);
        await context.SaveChangesAsync();
        if (request.UserId is null) return report;
        var reportResult = await context.Reports
            .Include(x => x.CreatedByNavigation)
            .FirstOrDefaultAsync(x => x.Id.Equals(report.Id));
        return reportResult!;
    }
}