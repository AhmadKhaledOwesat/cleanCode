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
    public class GoogleCommandController(IGoogleCommandBll countryBll, IDcpMapper mapper) : BaseController<GoogleCommand, GoogleCommandDto, Guid, GoogleCommandFilter>(countryBll, mapper)
    {
        public override async Task<DcpResponse<PageResult<GoogleCommandDto>>> GetAllAsync([FromBody] GoogleCommandFilter searchParameters)=> new DcpResponse<PageResult<GoogleCommandDto>>(mapper.Map<PageResult<GoogleCommandDto>>(await countryBll.GetAllAsync(searchParameters)));
    }
}
