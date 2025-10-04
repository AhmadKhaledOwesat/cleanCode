using DcpTracker.Application.Dto;
using DcpTracker.Domain.Entities;
using DcpTracker.Domain.Entities.Filters;
using DcpTracker.Domain.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Version = DcpTracker.Domain.Entities.Version;

namespace DcpTracker.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowAllOrigins")]
    public class VersionController(IVersionBll versionBll,IDcpMapper mapper) : BaseController<Version,VersionDto,Guid,VersionFilter>(versionBll, mapper)
    {
        public override async Task<DcpResponse<PageResult<VersionDto>>> GetAllAsync([FromBody] VersionFilter searchParameters)
        {
            return  new DcpResponse<PageResult<VersionDto>>(mapper.Map<PageResult<VersionDto>>(await versionBll.GetAllAsync(searchParameters)));      
        }
    }
}
