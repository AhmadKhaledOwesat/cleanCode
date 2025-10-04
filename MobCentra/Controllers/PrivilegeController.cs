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
    public class PrivilegeController(IPrivilegeBll PrivilegeBll,IDcpMapper mapper) : BaseController<Privilege,PrivilegeDto,Guid,PrivilegeFilter>(PrivilegeBll, mapper)
    {
        public override async Task<DcpResponse<PageResult<PrivilegeDto>>> GetAllAsync([FromBody] PrivilegeFilter searchParameters)
        {
            return  new DcpResponse<PageResult<PrivilegeDto>>(mapper.Map<PageResult<PrivilegeDto>>(await PrivilegeBll.GetAllAsync(searchParameters)));      
        }
    }
}
