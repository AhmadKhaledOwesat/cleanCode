using DcpTracker.Application.Dto;
using DcpTracker.Domain.Entities;
using DcpTracker.Domain.Entities.Filters;
using DcpTracker.Domain.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace DcpTracker.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowAllOrigins")]
    public class UserRoleController(IUserRoleBll UserRoleBll,IDcpMapper mapper) : BaseController<UserRole,UserRoleDto,Guid,UserRoleFilter>(UserRoleBll, mapper)
    {
        public override async Task<DcpResponse<PageResult<UserRoleDto>>> GetAllAsync([FromBody] UserRoleFilter searchParameters)
        {
            return  new DcpResponse<PageResult<UserRoleDto>>(mapper.Map<PageResult<UserRoleDto>>(await UserRoleBll.GetAllAsync(searchParameters)));      
        }
    }
}
