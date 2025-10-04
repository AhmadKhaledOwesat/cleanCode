using MobCentra.Domain.Entities;
using MobCentra.Domain.Entities.Filters;

namespace MobCentra.Domain.Interfaces
{
    public interface IUserRoleBll : IBaseBll<UserRole, Guid, UserRoleFilter>
    {
    }
}
