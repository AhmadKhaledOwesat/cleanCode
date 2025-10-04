using MobCentra.Application.Dto;
using MobCentra.Domain.Entities;
using MobCentra.Domain.Entities.Filters;
using MobCentra.Domain.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace MobCentra.Controllers
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
