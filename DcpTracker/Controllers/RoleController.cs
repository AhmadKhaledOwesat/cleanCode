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
    public class RoleController(IRoleBll RoleBll,IDcpMapper mapper) : BaseController<Role,RoleDto,Guid,RoleFilter>(RoleBll, mapper)
    {
        public override async Task<DcpResponse<PageResult<RoleDto>>> GetAllAsync([FromBody] RoleFilter searchParameters)
        {
            return  new DcpResponse<PageResult<RoleDto>>(mapper.Map<PageResult<RoleDto>>(await RoleBll.GetAllAsync(searchParameters)));      
        }
    }
}
