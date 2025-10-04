using DcpTracker.Domain.Entities.Filters;
using DcpTracker.Domain.Interfaces;
using DcpTracker.Infrastructure.Extensions;
using Group = DcpTracker.Domain.Entities.Group;

namespace DcpTracker.Application.Bll
{
    public class GroupBll(IBaseDal<Group, Guid, GroupFilter> baseDal,IConstraintBll constraintBll) : BaseBll<Group, Guid, GroupFilter>(baseDal), IGroupBll
    {
        public override Task<PageResult<Group>> GetAllAsync(GroupFilter searchParameters)
        {
            if (searchParameters is not null)
            {
                    searchParameters.Expression = new Func<Group, bool>(a => 
                    
                    (a.NameAr == searchParameters?.Description || searchParameters.Description.IsNullOrEmpty()) 
                    && a.CompanyId == searchParameters.CompanyId
                    && (searchParameters.Active == null || a.Active == searchParameters.Active));
            }

            return base.GetAllAsync(searchParameters);
        }

        public override async Task AddAsync(Group entity)
        {
            await constraintBll.GetLimitAsync(entity.CompanyId.Value, Domain.Enum.LimitType.NoOfGroups);
            await base.AddAsync(entity);
        }
    }
}
