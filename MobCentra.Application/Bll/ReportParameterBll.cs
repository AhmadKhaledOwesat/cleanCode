using MobCentra.Domain.Entities;
using MobCentra.Domain.Entities.Filters;
using MobCentra.Domain.Interfaces;

namespace MobCentra.Application.Bll
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
