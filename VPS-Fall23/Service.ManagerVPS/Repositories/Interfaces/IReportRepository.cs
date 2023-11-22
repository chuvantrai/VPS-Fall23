using Service.ManagerVPS.DTO.Input;
using Service.ManagerVPS.DTO.OtherModels;
using Service.ManagerVPS.Models;

namespace Service.ManagerVPS.Repositories.Interfaces;

public interface IReportRepository : IVpsRepository<Report>
{
    Task<Report> CreateReport(CreateReportRequest request);
    
    Task<int?> CheckPaymentCodeInReport(string paymentCode,int type);
    PagedList<Report> GetListReportForAdmin(QueryStringParameters parameters);
    List<GlobalStatus> GetTypeReport();
    PagedList<Report> FilterReportForAdmin(QueryStringParameters parameters, int typeId);
}