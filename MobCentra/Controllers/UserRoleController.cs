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
    public class UserRoleController(IUserRoleBll userRoleBll, IDcpMapper mapper) : BaseController<UserRole, UserRoleDto, Guid, UserRoleFilter>(userRoleBll, mapper)
    {
        public override async Task<DcpResponse<PageResult<UserRoleDto>>> GetAllAsync([FromBody] UserRoleFilter searchParameters)
        {
            return new DcpResponse<PageResult<UserRoleDto>>(mapper.Map<PageResult<UserRoleDto>>(await userRoleBll.GetAllAsync(searchParameters)));
        }
    }
}
