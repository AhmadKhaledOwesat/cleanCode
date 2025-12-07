using MobCentra.Domain.Entities;
using MobCentra.Domain.Entities.Filters;
using MobCentra.Domain.Interfaces;
using MobCentra.Infrastructure.Extensions;

namespace MobCentra.Application.Bll
{
    /// <summary>
    /// Business logic layer for privilege management operations
    /// </summary>
    public class PrivilegeBll(IBaseDal<Privilege, Guid, PrivilegeFilter> baseDal) : BaseBll<Privilege, Guid, PrivilegeFilter>(baseDal), IPrivilegeBll
    {
        /// <summary>
        /// Retrieves privileges with filtering by keyword and sorted by sort order
        /// </summary>
        /// <param name="searchParameters">Filter parameters for searching and pagination</param>
        /// <returns>Paginated result containing matching privileges sorted by sort order</returns>
        public override async Task<PageResult<Privilege>> GetAllAsync(PrivilegeFilter searchParameters)
        {
            // Build search expression with keyword filter if name is provided
            if (searchParameters is not null)
            {
                if (!string.IsNullOrEmpty(searchParameters.Name))
                    searchParameters.Expression = new Func<Privilege, bool>(a => (searchParameters.Keyword.IsNullOrEmpty() || a.PrivilegeName.Contains(searchParameters?.Keyword)));
            }

            var data = await base.GetAllAsync(searchParameters);
            // Sort results by sort order
            data.Collections = [.. data.Collections.OrderBy(a => a.SortOrder)];
            return data;
        }
    }
}
