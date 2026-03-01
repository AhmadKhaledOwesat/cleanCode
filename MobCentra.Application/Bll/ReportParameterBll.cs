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
        public async Task<dynamic> GetListDataByIdAsync(Guid id,Guid companyId)
        {
            ReportParameter reportParameters = await GetByIdAsync(id);
            dynamic mbDropdownOptions = await baseDal.ExecuteSqlAsync(reportParameters.ParameterQuery.Replace("@CompanyId",companyId.ToString()));
            return mbDropdownOptions;
        }
    }
}
