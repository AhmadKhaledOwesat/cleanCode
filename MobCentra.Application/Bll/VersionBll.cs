using MobCentra.Domain.Entities.Filters;
using MobCentra.Domain.Interfaces;
using MobCentra.Infrastructure.Extensions;
using Version = MobCentra.Domain.Entities.Version;

namespace MobCentra.Application.Bll
{
    /// <summary>
    /// Business logic layer for version management operations
    /// </summary>
    public class VersionBll(IBaseDal<Version, Guid, VersionFilter> baseDal) : BaseBll<Version, Guid, VersionFilter>(baseDal), IVersionBll
    {
        /// <summary>
        /// Retrieves versions with optional filtering by version name
        /// </summary>
        /// <param name="searchParameters">Filter parameters for searching and pagination</param>
        /// <returns>Paginated result containing matching versions</returns>
        public override Task<PageResult<Version>> GetAllAsync(VersionFilter searchParameters)
        {
            // Build search expression filtering by version name if provided
            if (searchParameters is not null)
            {
                if (!searchParameters.Name.IsNullOrEmpty())
                    searchParameters.Expression = new Func<Version, bool>(a => a.VersionName == searchParameters?.Name);
            }

            return base.GetAllAsync(searchParameters);
        }
    }
}
