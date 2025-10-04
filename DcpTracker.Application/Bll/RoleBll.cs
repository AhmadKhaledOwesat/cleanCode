using MobCentra.Domain.Entities;
using MobCentra.Domain.Entities.Filters;
using MobCentra.Domain.Interfaces;
using MobCentra.Infrastructure.Extensions;
using System.Linq.Expressions;

namespace MobCentra.Application.Bll
{
    public class RoleBll(IBaseDal<Role, Guid, RoleFilter> baseDal, IRolePrivilegeBll rolePrivilegeBll, IConstraintBll constraintBll) : BaseBll<Role, Guid, RoleFilter>(baseDal), IRoleBll
    {
        public override Task<PageResult<Role>> GetAllAsync(RoleFilter searchParameters)
        {
            if (searchParameters is not null)
            {
                searchParameters.Expression = new Func<Role, bool>(a =>
                (a.NameAr == searchParameters?.Name || searchParameters.Name.IsNullOrEmpty())
                && a.CompanyId == searchParameters.CompanyId &&
                (searchParameters.Active == null || a.Active == searchParameters.Active));
            }

            return base.GetAllAsync(searchParameters);
        }
        public override async Task AddAsync(Role entity)
        {
            await constraintBll.GetLimitAsync(entity.CompanyId.Value, Domain.Enum.LimitType.NoOfRoles);
            await base.AddAsync(entity);
        }
        public override async Task UpdateAsync(Role entity)
        {
            await HandleRolePrivilages(entity);
            await base.UpdateAsync(entity);
        }
        private async Task HandleRolePrivilages(Role entity)
        {
            Expression<Func<RolePrivilege, bool>> expression = x => x.RoleId == entity.Id;
            List<RolePrivilege> rolePrivileges = await rolePrivilegeBll.FindAllByExpressionAsync(expression);
            if (rolePrivileges.Count > 0)
                await rolePrivilegeBll.DeleteRangeAsync(rolePrivileges);
            foreach (var item in entity.RolePrivileges)
            {
                item.Role = null;
                item.Id = default;
            }
            await rolePrivilegeBll.AddRangeAsync([.. entity.RolePrivileges]);
        }
    }
}
