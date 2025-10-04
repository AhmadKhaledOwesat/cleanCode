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
    public class PrivilegeController(IPrivilegeBll PrivilegeBll,IDcpMapper mapper) : BaseController<Privilege,PrivilegeDto,Guid,PrivilegeFilter>(PrivilegeBll, mapper)
    {
        public override async Task<DcpResponse<PageResult<PrivilegeDto>>> GetAllAsync([FromBody] PrivilegeFilter searchParameters)
        {
            return  new DcpResponse<PageResult<PrivilegeDto>>(mapper.Map<PageResult<PrivilegeDto>>(await PrivilegeBll.GetAllAsync(searchParameters)));      
        }
    }
}
