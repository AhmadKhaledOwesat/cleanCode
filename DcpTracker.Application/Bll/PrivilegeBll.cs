using DcpTracker.Domain.Entities;
using DcpTracker.Domain.Entities.Filters;
using DcpTracker.Domain.Interfaces;

namespace DcpTracker.Application.Bll
{
    public class PrivilegeBll(IBaseDal<Privilege, Guid, PrivilegeFilter> baseDal) : BaseBll<Privilege, Guid, PrivilegeFilter>(baseDal), IPrivilegeBll
    {
        public override Task<PageResult<Privilege>> GetAllAsync(PrivilegeFilter searchParameters)
        {
            if (searchParameters is not null)
            {
                if (!string.IsNullOrEmpty(searchParameters.Name))
                    searchParameters.Expression = new Func<Privilege, bool>(a => a.PrivilegeName == searchParameters.Name);
            }

            return base.GetAllAsync(searchParameters);
        }
    }
}
