using MobCentra.Domain.Entities;
using MobCentra.Domain.Entities.Filters;
using MobCentra.Domain.Interfaces;

namespace MobCentra.Application.Bll
{
    public class UserGroupBll(IBaseDal<UserGroup, Guid, UserGroupFilter> baseDal) : BaseBll<UserGroup, Guid, UserGroupFilter>(baseDal), IUserGroupBll
    {
        public override Task<PageResult<UserGroup>> GetAllAsync(UserGroupFilter searchParameters)
        {
            return base.GetAllAsync(searchParameters);
        }
    }
}
