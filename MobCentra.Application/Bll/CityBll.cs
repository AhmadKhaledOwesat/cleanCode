using MobCentra.Domain.Entities;
using MobCentra.Domain.Entities.Filters;
using MobCentra.Domain.Interfaces;
using MobCentra.Infrastructure.Extensions;

namespace MobCentra.Application.Bll
{
    /// <summary>
    /// Business logic layer for city/area management operations
    /// </summary>
    public class CityBll(IBaseDal<City, Guid, CityFilter> baseDal, IConstraintBll constraintBll) : BaseBll<City, Guid, CityFilter>(baseDal), ICityBll
    {
        /// <summary>
        /// Retrieves cities with filtering by keyword (name in Arabic or Other) and company
        /// </summary>
        /// <param name="searchParameters">Filter parameters for searching and pagination</param>
        /// <returns>Paginated result containing matching cities</returns>
        public override async Task<PageResult<City>> GetAllAsync(CityFilter searchParameters)
        {
            // Build search expression with keyword and company filters
            if (searchParameters is not null)
            {
                searchParameters.Expression = new Func<City, bool>(a =>
                    (searchParameters.Keyword.IsNullOrEmpty() || (a.Name.Contains(searchParameters?.Keyword) || a.NameOt.Contains(searchParameters.Keyword))) &&
                a.CompanyId == searchParameters.CompanyId
                );
            }
            return await base.GetAllAsync(searchParameters);
        }

        /// <summary>
        /// Adds a new city/area after validating company area limits
        /// </summary>
        /// <param name="entity">The city entity to add</param>
        public override async Task AddAsync(City entity)
        {
            // Check if company has reached the maximum number of areas limit
            await constraintBll.GetLimitAsync(entity.CompanyId.Value, Domain.Enum.LimitType.NoOfArea);
            await base.AddAsync(entity);
        }
    }
}
