using MobCentra.Domain.Entities.Filters;
using MobCentra.Domain.Interfaces;
using MobCentra.Infrastructure.Extensions;

namespace MobCentra.Application.Bll
{
    /// <summary>
    /// Business logic layer for application management operations
    /// </summary>
    public class ApplicationBll(IBaseDal<MobCentra.Domain.Entities.Application, Guid, ApplicationFilter> baseDal, IConstraintBll constraintBll) : BaseBll<MobCentra.Domain.Entities.Application, Guid, ApplicationFilter>(baseDal), IApplicationBll
    {
        /// <summary>
        /// Retrieves applications with filtering by keyword (name in Arabic or Other), company, and active status
        /// </summary>
        /// <param name="searchParameters">Filter parameters for searching and pagination</param>
        /// <returns>Paginated result containing matching applications</returns>
        public override async Task<PageResult<MobCentra.Domain.Entities.Application>> GetAllAsync(ApplicationFilter searchParameters)
        {
            // Build search expression with keyword, company, and active status filters
            if (searchParameters is not null)
            {
                searchParameters.Expression = new Func<MobCentra.Domain.Entities.Application, bool>(a =>
                ((searchParameters.Keyword.IsNullOrEmpty() || a.NameAr.Contains(searchParameters?.Keyword)) || (searchParameters.Keyword.IsNullOrEmpty() || a.NameOt.Contains(searchParameters?.Keyword)))
                && a.CompanyId == searchParameters.CompanyId
                && (searchParameters.Active == null || a.Active == searchParameters.Active));
            }
            return await base.GetAllAsync(searchParameters);
        }

        /// <summary>
        /// Adds a new application after validating constraints and uploading application file if provided
        /// </summary>
        /// <param name="entity">The application entity to add</param>
        public override async Task AddAsync(Domain.Entities.Application entity)
        {
            // Check if company has reached the maximum number of applications limit
            await constraintBll.GetLimitAsync(entity.CompanyId.Value, Domain.Enum.LimitType.NoOfApplications);

            // Upload application file if provided
            if (!entity.File.IsNullOrEmpty())
                entity.File = await entity.File.UplodaFiles();
            await base.AddAsync(entity);
        }

        /// <summary>
        /// Updates an existing application, handling file upload if file has changed
        /// </summary>
        /// <param name="entity">The application entity with updated information</param>
        public override async Task UpdateAsync(Domain.Entities.Application entity)
        {
            // Get existing application to compare file
            var app = await GetByIdAsync(entity.Id);

            // Upload new file if file has changed and is not empty
            if (app.File != entity.File && !entity.File.IsNullOrEmpty())
                entity.File = await app.File.UplodaFiles();
            else
                entity.File = app.File;
            await base.UpdateAsync(entity);
        }
    }
}
