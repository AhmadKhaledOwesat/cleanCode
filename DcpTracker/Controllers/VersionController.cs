using MobCentra.Application.Dto;
using MobCentra.Domain.Entities;
using MobCentra.Domain.Entities.Filters;
using MobCentra.Domain.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Version = MobCentra.Domain.Entities.Version;

namespace MobCentra.Controllers
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
