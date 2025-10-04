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
    public class GoogleCommandController(IGoogleCommandBll countryBll, IDcpMapper mapper) : BaseController<GoogleCommand, GoogleCommandDto, Guid, GoogleCommandFilter>(countryBll, mapper)
    {
        public override async Task<DcpResponse<PageResult<GoogleCommandDto>>> GetAllAsync([FromBody] GoogleCommandFilter searchParameters)=> new DcpResponse<PageResult<GoogleCommandDto>>(mapper.Map<PageResult<GoogleCommandDto>>(await countryBll.GetAllAsync(searchParameters)));
    }
}
