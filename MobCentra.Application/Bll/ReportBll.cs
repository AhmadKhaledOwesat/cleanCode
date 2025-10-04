using MobCentra.Application.Interfaces;
using MobCentra.Domain.Entities;
using MobCentra.Domain.Entities.Filters;
using MobCentra.Domain.Interfaces;
using MobCentra.Infrastructure.Extensions;

namespace MobCentra.Application.Bll
{
    public class ReportBll(IBaseDal<Report, Guid, ReportFilter> baseDal) : BaseBll<Report, Guid, ReportFilter>(baseDal), IReportBll
    {
        public override Task<PageResult<Report>> GetAllAsync(ReportFilter searchParameters)
        {
            if (searchParameters is not null)
            {
                    searchParameters.Expression = new Func<Report, bool>(a => 
                    (a.ReportName == searchParameters?.Name || searchParameters.Name.IsNullOrEmpty())
                    && a.CompanyId == searchParameters.CompanyId
                    );
            }

            return base.GetAllAsync(searchParameters);
        }

        public async Task<DcpResponse<dynamic>> ExecuteReport(string query)
        {
            dynamic data = await baseDal.ExecuteSQL(query);
            return new DcpResponse<dynamic>(data);
        }
    }
}
