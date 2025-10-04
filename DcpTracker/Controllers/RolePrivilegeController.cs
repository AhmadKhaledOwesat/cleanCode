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
    public class RolePrivilegeController(IRolePrivilegeBll RolePrivilegeBll,IDcpMapper mapper) : BaseController<RolePrivilege,RolePrivilegeDto,Guid,RolePrivilegeFilter>(RolePrivilegeBll, mapper)
    {
        public override async Task<DcpResponse<PageResult<RolePrivilegeDto>>> GetAllAsync([FromBody] RolePrivilegeFilter searchParameters)
        {
            return  new DcpResponse<PageResult<RolePrivilegeDto>>(mapper.Map<PageResult<RolePrivilegeDto>>(await RolePrivilegeBll.GetAllAsync(searchParameters)));      
        }
    }
}
