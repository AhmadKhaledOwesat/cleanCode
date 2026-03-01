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
    public class MDMCommandController(IMDMCommandBll countryBll, IDcpMapper mapper) : BaseController<MDMCommand, MDMCommandDto, Guid, MDMCommandFilter>(countryBll, mapper)
    {
        public override async Task<DcpResponse<PageResult<MDMCommandDto>>> GetAllAsync([FromBody] MDMCommandFilter searchParameters)=> new DcpResponse<PageResult<MDMCommandDto>>(mapper.Map<PageResult<MDMCommandDto>>(await countryBll.GetAllAsync(searchParameters)));
    }
}
