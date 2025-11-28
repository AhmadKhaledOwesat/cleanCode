using MobCentra.Domain.Entities;
using MobCentra.Domain.Entities.Filters;
using MobCentra.Domain.Interfaces;

namespace MobCentra.Application.Bll
{
    public class PrivilegeBll(IBaseDal<Privilege, Guid, PrivilegeFilter> baseDal) : BaseBll<Privilege, Guid, PrivilegeFilter>(baseDal), IPrivilegeBll
    {
        public override async Task<PageResult<Privilege>> GetAllAsync(PrivilegeFilter searchParameters)
        {
            if (searchParameters is not null)
            {
                if (!string.IsNullOrEmpty(searchParameters.Name))
                    searchParameters.Expression = new Func<Privilege, bool>(a => a.PrivilegeName == searchParameters.Name);
            }

            var data = await base.GetAllAsync(searchParameters);
            data.Collections = [.. data.Collections.OrderBy(a => a.SortOrder)];
            return data;
        }
    }
}
