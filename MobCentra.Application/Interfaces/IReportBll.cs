using MobCentra.Domain.Entities;
using MobCentra.Domain.Entities.Filters;
using MobCentra.Domain.Interfaces;

namespace MobCentra.Application.Interfaces
{
    public interface IReportBll : IBaseBll<Report, Guid, ReportFilter>
    {
        Task<DcpResponse<dynamic>> ExecuteReport(string query);
    }
}
