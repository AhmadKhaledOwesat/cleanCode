using DcpTracker.Domain.Entities;
using DcpTracker.Domain.Entities.Filters;

namespace DcpTracker.Domain.Interfaces
{
    public interface IUserGroupBll : IBaseBll<UserGroup, Guid, UserGroupFilter>
    {
    }
}
