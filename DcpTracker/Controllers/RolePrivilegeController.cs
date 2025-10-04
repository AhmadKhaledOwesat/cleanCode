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
    public class RolePrivilegeController(IRolePrivilegeBll RolePrivilegeBll,IDcpMapper mapper) : BaseController<RolePrivilege,RolePrivilegeDto,Guid,RolePrivilegeFilter>(RolePrivilegeBll, mapper)
    {
        public override async Task<DcpResponse<PageResult<RolePrivilegeDto>>> GetAllAsync([FromBody] RolePrivilegeFilter searchParameters)
        {
            return  new DcpResponse<PageResult<RolePrivilegeDto>>(mapper.Map<PageResult<RolePrivilegeDto>>(await RolePrivilegeBll.GetAllAsync(searchParameters)));      
        }
    }
}
