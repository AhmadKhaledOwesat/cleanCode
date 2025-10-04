using MobCentra.Domain.Entities;
using MobCentra.Domain.Entities.Filters;

namespace MobCentra.Domain.Interfaces
{
    public interface IRolePrivilegeBll : IBaseBll<RolePrivilege, Guid, RolePrivilegeFilter>
    {
    }
}
