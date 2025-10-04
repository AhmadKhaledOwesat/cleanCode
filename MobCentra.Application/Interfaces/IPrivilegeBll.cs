using MobCentra.Domain.Entities;
using MobCentra.Domain.Entities.Filters;

namespace MobCentra.Domain.Interfaces
{
    public interface IPrivilegeBll : IBaseBll<Privilege, Guid, PrivilegeFilter>
    {
    }
}
