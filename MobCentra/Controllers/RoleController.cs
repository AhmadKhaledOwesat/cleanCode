using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MobCentra.Application.Dto;
using MobCentra.Domain.Entities;
using MobCentra.Domain.Entities.Filters;
using MobCentra.Domain.Interfaces;

namespace MobCentra.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowAllOrigins")]
    public class RoleController(IRoleBll RoleBll, IDcpMapper mapper) : BaseController<Role, RoleDto, Guid, RoleFilter>(RoleBll, mapper)
    {
        public override async Task<DcpResponse<PageResult<RoleDto>>> GetAllAsync([FromBody] RoleFilter searchParameters)
        {
            if (!searchParameters.IsByPass && !await RoleBll.IsAuthorizedAsync(Guid.Parse(Permissions.UserRoles)))
                throw new UnauthorizedAccessException();

            return new DcpResponse<PageResult<RoleDto>>(mapper.Map<PageResult<RoleDto>>(await RoleBll.GetAllAsync(searchParameters)));
        }
    }
}
