using DcpTracker.Domain.Entities;
using DcpTracker.Domain.Entities.Filters;
using DcpTracker.Domain.Interfaces;

namespace DcpTracker.Application.Bll
{
    public class RolePrivilegeBll(IBaseDal<RolePrivilege, Guid, RolePrivilegeFilter> baseDal) : BaseBll<RolePrivilege, Guid, RolePrivilegeFilter>(baseDal), IRolePrivilegeBll
    {
        public override Task<PageResult<RolePrivilege>> GetAllAsync(RolePrivilegeFilter searchParameters)
        {
            searchParameters.Expression = new Func<RolePrivilege, bool>(x => x.RoleId == searchParameters.RoleId);
            return base.GetAllAsync(searchParameters);
        }
    }
}
