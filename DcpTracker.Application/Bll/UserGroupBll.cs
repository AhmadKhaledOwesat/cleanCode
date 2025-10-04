using DcpTracker.Domain.Entities;
using DcpTracker.Domain.Entities.Filters;
using DcpTracker.Domain.Interfaces;

namespace DcpTracker.Application.Bll
{
    public class UserGroupBll(IBaseDal<UserGroup, Guid, UserGroupFilter> baseDal) : BaseBll<UserGroup, Guid, UserGroupFilter>(baseDal), IUserGroupBll
    {
        public override Task<PageResult<UserGroup>> GetAllAsync(UserGroupFilter searchParameters)
        {
            return base.GetAllAsync(searchParameters);
        }
    }
}
