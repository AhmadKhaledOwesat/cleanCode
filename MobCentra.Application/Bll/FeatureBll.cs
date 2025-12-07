using MobCentra.Domain.Entities.Filters;
using MobCentra.Domain.Interfaces;
using MobCentra.Infrastructure.Extensions;
using Feature = MobCentra.Domain.Entities.Feature;

namespace MobCentra.Application.Bll
{
    /// <summary>
    /// Business logic layer for feature management operations
    /// </summary>
    public class FeatureBll(IBaseDal<Feature, int, FeatureFilter> baseDal) : BaseBll<Feature, int, FeatureFilter>(baseDal), IFeatureBll
    {
        /// <summary>
        /// Retrieves active features with filtering by keyword (name in Arabic or Other)
        /// </summary>
        /// <param name="searchParameters">Filter parameters for searching and pagination</param>
        /// <returns>Paginated result containing matching active features</returns>
        public override Task<PageResult<Feature>> GetAllAsync(FeatureFilter searchParameters)
        {
            // Build search expression filtering only active features
            if (searchParameters is not null)
            {
                    searchParameters.Expression = new Func<Feature, bool>(a =>
                    (searchParameters.Keyword.IsNullOrEmpty() || a.NameAr.Contains(searchParameters?.Keyword) || a.NameOt.Contains(searchParameters?.Keyword))
                   && a.Active == 1);
            }

            return base.GetAllAsync(searchParameters);
        }
    }
}
