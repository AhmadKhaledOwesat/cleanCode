using DcpTracker.Domain.Entities;
using DcpTracker.Domain.Entities.Filters;
using DcpTracker.Domain.Interfaces;

namespace DcpTracker.Application.Bll
{
    public class ReportParameterBll(IBaseDal<ReportParameter, Guid, ReportParameterFilter> baseDal) : BaseBll<ReportParameter, Guid, ReportParameterFilter>(baseDal), IReportParameterBll
    {
        public override Task<PageResult<ReportParameter>> GetAllAsync(ReportParameterFilter searchParameters)
        {
            searchParameters.Expression = new Func<ReportParameter, bool>(a => a.ReportId == searchParameters.ReportId);
            return base.GetAllAsync(searchParameters);
        }
    }
}
