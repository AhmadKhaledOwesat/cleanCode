using MobCentra.Application.Interfaces;
using MobCentra.Domain.Entities;
using MobCentra.Domain.Entities.Filters;
using MobCentra.Domain.Interfaces;
using MobCentra.Infrastructure.Extensions;

namespace MobCentra.Application.Bll
{
    /// <summary>
    /// Business logic layer for report management operations
    /// </summary>
    public class ReportBll(IBaseDal<Report, Guid, ReportFilter> baseDal) : BaseBll<Report, Guid, ReportFilter>(baseDal), IReportBll
    {
        /// <summary>
        /// Retrieves reports with filtering by keyword (report name in Arabic or English) and company
        /// </summary>
        /// <param name="searchParameters">Filter parameters for searching and pagination</param>
        /// <returns>Paginated result containing matching reports</returns>
        public override Task<PageResult<Report>> GetAllAsync(ReportFilter searchParameters)
        {
            // Build search expression with keyword and company filters
            if (searchParameters is not null)
            {
                    searchParameters.Expression = new Func<Report, bool>(a =>
                    (searchParameters.Keyword.IsNullOrEmpty() || a.ReportName.Contains(searchParameters?.Keyword) || a.ReportNameEn.Contains(searchParameters?.Keyword))
                    );
            }

            return base.GetAllAsync(searchParameters);
        }

        /// <summary>
        /// Executes a SQL query and returns the results
        /// </summary>
        /// <param name="query">The SQL query string to execute</param>
        /// <returns>Response containing the query results</returns>
        public async Task<DcpResponse<dynamic>> ExecuteReport(string query)
        {
            // Execute SQL query directly
            dynamic data = await baseDal.ExecuteSqlAsync(query);
            return new DcpResponse<dynamic>(data);
        }
    }
}
