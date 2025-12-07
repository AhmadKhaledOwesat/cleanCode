using MobCentra.Domain.Entities;
using MobCentra.Domain.Entities.Filters;
using MobCentra.Domain.Interfaces;
using MobCentra.Infrastructure.Extensions;
using System.Linq.Expressions;

namespace MobCentra.Application.Bll
{
    /// <summary>
    /// Business logic layer for role management operations
    /// </summary>
    public class RoleBll(IBaseDal<Role, Guid, RoleFilter> baseDal, IRolePrivilegeBll rolePrivilegeBll, IConstraintBll constraintBll) : BaseBll<Role, Guid, RoleFilter>(baseDal), IRoleBll
    {
        /// <summary>
        /// Retrieves roles with filtering by keyword (name in Arabic or Other), company, and active status
        /// </summary>
        /// <param name="searchParameters">Filter parameters for searching and pagination</param>
        /// <returns>Paginated result containing matching roles</returns>
        public override Task<PageResult<Role>> GetAllAsync(RoleFilter searchParameters)
        {
            // Build search expression with keyword, company, and active status filters
            if (searchParameters is not null)
            {
                searchParameters.Expression = new Func<Role, bool>(a =>
                (searchParameters.Keyword.IsNullOrEmpty() || a.NameAr.Contains(searchParameters?.Keyword) || a.NameOt.Contains(searchParameters?.Keyword))
                && a.CompanyId == searchParameters.CompanyId &&
                (searchParameters.Active == null || a.Active == searchParameters.Active));
            }

            return base.GetAllAsync(searchParameters);
        }
        
        /// <summary>
        /// Adds a new role after validating company role limits
        /// </summary>
        /// <param name="entity">The role entity to add</param>
        public override async Task AddAsync(Role entity)
        {
            // Check if company has reached the maximum number of roles limit
            await constraintBll.GetLimitAsync(entity.CompanyId.Value, Domain.Enum.LimitType.NoOfRoles);
            await base.AddAsync(entity);
        }
        
        /// <summary>
        /// Updates a role and handles role privileges updates
        /// </summary>
        /// <param name="entity">The role entity with updated information</param>
        public override async Task UpdateAsync(Role entity)
        {
            // Update role privileges before updating role
            await HandleRolePrivilages(entity);
            await base.UpdateAsync(entity);
        }
        
        /// <summary>
        /// Handles updating role privileges by removing existing privileges and adding new ones
        /// </summary>
        /// <param name="entity">The role entity containing new privileges</param>
        private async Task HandleRolePrivilages(Role entity)
        {
            // Find all existing role privileges
            Expression<Func<RolePrivilege, bool>> expression = x => x.RoleId == entity.Id;
            List<RolePrivilege> rolePrivileges = await rolePrivilegeBll.FindAllByExpressionAsync(expression);
            
            // Delete existing privileges if any
            if (rolePrivileges.Count > 0)
                await rolePrivilegeBll.DeleteRangeAsync(rolePrivileges);
            
            // Clear navigation properties and reset IDs for new privileges
            foreach (var item in entity.RolePrivileges)
            {
                item.Role = null;
                item.Id = default;
            }
            await rolePrivilegeBll.AddRangeAsync([.. entity.RolePrivileges]);
        }
    }
}
