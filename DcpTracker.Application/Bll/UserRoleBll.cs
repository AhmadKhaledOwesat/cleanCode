using DcpTracker.Domain.Entities;
using DcpTracker.Domain.Entities.Filters;
using DcpTracker.Domain.Interfaces;

namespace DcpTracker.Application.Bll
{
    public class UserRoleBll(IBaseDal<UserRole, Guid, UserRoleFilter> baseDal) : BaseBll<UserRole, Guid, UserRoleFilter>(baseDal), IUserRoleBll
    {
        public override Task<PageResult<UserRole>> GetAllAsync(UserRoleFilter searchParameters)
        {
            return base.GetAllAsync(searchParameters);
        }

    }
}
