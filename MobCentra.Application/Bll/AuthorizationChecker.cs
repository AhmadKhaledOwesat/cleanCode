using Microsoft.EntityFrameworkCore;
using MobCentra.Application.Interfaces;
using MobCentra.Infrastructure.EfContext;

namespace MobCentra.Application.Bll
{
    public class AuthorizationChecker(StudioContext context) : IAuthorizationChecker
    {
        public async Task<bool> HasPermissionAsync(Guid userId, Guid privilegeId)
        {
            return await context.UserRoles
                .Include(ur => ur.Role)
                .ThenInclude(r => r!.RolePrivileges)
                .Where(ur => ur.UserId == userId && ur.Role != null && ur.Role.Active == 1)
                .AnyAsync(ur => ur.Role!.RolePrivileges != null && ur.Role.RolePrivileges.Any(rp => rp.PrivilegeId == privilegeId));
        }
    }
}
