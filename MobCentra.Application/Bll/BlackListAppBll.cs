using MobCentra.Domain.Entities;
using MobCentra.Domain.Entities.Filters;
using MobCentra.Domain.Interfaces;
using MobCentra.Infrastructure.Extensions;

namespace MobCentra.Application.Bll
{
    /// <summary>
    /// Business logic layer for blacklisted application management operations
    /// </summary>
    public class BlackListAppBll(IBaseDal<BlackListApp, Guid, BlackListAppFilter> baseDal) : BaseBll<BlackListApp, Guid, BlackListAppFilter>(baseDal), IBlackListAppBll
    {
        /// <summary>
        /// Retrieves blacklisted applications with filtering by keyword and company
        /// </summary>
        /// <param name="searchParameters">Filter parameters for searching and pagination</param>
        /// <returns>Paginated result containing matching blacklisted applications</returns>
        public override Task<PageResult<BlackListApp>> GetAllAsync(BlackListAppFilter searchParameters)
        {
            // Build search expression with keyword and company filters
            if (searchParameters is not null)
            {
                    searchParameters.Expression = new Func<BlackListApp, bool>(a =>
                    (a.Name.Contains(searchParameters?.Keyword) || searchParameters.Keyword.IsNullOrEmpty())
                    && a.CompanyId == searchParameters.CompanyId

                    );

            }

            return base.GetAllAsync(searchParameters);
        }
    }
}
