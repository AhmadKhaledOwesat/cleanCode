using DcpTracker.Application.Dto;
using DcpTracker.Domain.Entities;
using DcpTracker.Domain.Entities.Filters;

namespace DcpTracker.Domain.Interfaces
{
    public interface IReportParameterBll : IBaseBll<ReportParameter, Guid, ReportParameterFilter>
    {
    }
}
