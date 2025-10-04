using DcpTracker.Domain.Entities;
using DcpTracker.Domain.Entities.Filters;
using DcpTracker.Domain.Interfaces;

namespace DcpTracker.Application.Interfaces
{
    public interface IReportBll : IBaseBll<Report, Guid, ReportFilter>
    {
        Task<DcpResponse<dynamic>> ExecuteReport(string query);
    }
}
