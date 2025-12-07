using MobCentra.Application.Interfaces;
using MobCentra.Domain.Entities.Filters;
using MobCentra.Domain.Interfaces;
using MobCentra.Infrastructure.Extensions;
using Group = MobCentra.Domain.Entities.Group;

namespace MobCentra.Application.Bll
{
    /// <summary>
    /// Business logic layer for group management operations
    /// </summary>
    public class GroupBll(IBaseDal<Group, Guid, GroupFilter> baseDal,IConstraintBll constraintBll,IIdentityManager<Guid> identityManager,IUserGroupBll userGroupBll) : BaseBll<Group, Guid, GroupFilter>(baseDal), IGroupBll
    {
        /// <summary>
        /// Retrieves groups with filtering by keyword (name in Arabic or Other), company, and active status
        /// </summary>
        /// <param name="searchParameters">Filter parameters for searching and pagination</param>
        /// <returns>Paginated result containing matching groups</returns>
        public override Task<PageResult<Group>> GetAllAsync(GroupFilter searchParameters)
        {
            // Build search expression with keyword, company, and active status filters
            if (searchParameters is not null)
            {
                    searchParameters.Expression = new Func<Group, bool>(a => 
                    
                    (searchParameters.Keyword.IsNullOrEmpty() || a.NameAr.Contains(searchParameters?.Keyword) || a.NameOt.Contains(searchParameters?.Keyword)) 
                    && a.CompanyId == searchParameters.CompanyId
                    && (searchParameters.Active == null || a.Active == searchParameters.Active));
            }

            return base.GetAllAsync(searchParameters);
        }

        /// <summary>
        /// Adds a new group after validating company group limits
        /// </summary>
        /// <param name="entity">The group entity to add</param>
        public override async Task AddAsync(Group entity)
        {
            var userId = identityManager.CurrentUserId;
            // Check if company has reached the maximum number of groups limit
            await constraintBll.GetLimitAsync(entity.CompanyId.Value, Domain.Enum.LimitType.NoOfGroups);
            await base.AddAsync(entity);
            await userGroupBll.AddAsync(new Domain.Entities.UserGroup { GroupId = entity.Id , Group = null , User =null , UserId = userId });
        }
    }
}
