using Service.ManagerVPS.DTO.Input;
using Service.ManagerVPS.Models;

namespace Service.ManagerVPS.Repositories.Interfaces;

public interface IReportRepository : IVpsRepository<Report>
{
    Task<Report> CreateReport(CreateReportRequest request);
}