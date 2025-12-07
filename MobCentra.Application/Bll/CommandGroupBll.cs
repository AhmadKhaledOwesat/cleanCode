using MobCentra.Domain.Entities.Filters;
using MobCentra.Domain.Interfaces;
using MobCentra.Infrastructure.Extensions;
using CommandGroup = MobCentra.Domain.Entities.CommandGroup;

namespace MobCentra.Application.Bll
{
    /// <summary>
    /// Business logic layer for command group management operations
    /// </summary>
    public class CommandGroupBll(IBaseDal<CommandGroup, Guid, CommandGroupFilter> baseDal) : BaseBll<CommandGroup, Guid, CommandGroupFilter>(baseDal), ICommandGroupBll
    {
        /// <summary>
        /// Retrieves command groups with filtering by description (name in Arabic or Other) and active status
        /// </summary>
        /// <param name="searchParameters">Filter parameters for searching and pagination</param>
        /// <returns>Paginated result containing matching command groups</returns>
        public override Task<PageResult<CommandGroup>> GetAllAsync(CommandGroupFilter searchParameters)
        {
            // Build search expression with description and active status filters
            if (searchParameters is not null)
            {
                    searchParameters.Expression = new Func<CommandGroup, bool>(a => 
                    
                    (searchParameters.Description.IsNullOrEmpty() ||
                     a.NameAr.Contains(searchParameters?.Description) ||
                     a.NameOt.Contains(searchParameters?.Description)
                    ) 
                    && (searchParameters.Active == null || a.Active == searchParameters.Active));
            }

            return base.GetAllAsync(searchParameters);
        }
    }
}
