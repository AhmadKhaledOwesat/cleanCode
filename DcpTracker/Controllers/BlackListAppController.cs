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
    public class BlackListAppController(IBlackListAppBll countryBll, IDcpMapper mapper) : BaseController<BlackListApp, BlackListAppDto, Guid, BlackListAppFilter>(countryBll, mapper)
    {
        public override async Task<DcpResponse<PageResult<BlackListAppDto>>> GetAllAsync([FromBody] BlackListAppFilter searchParameters)=> new DcpResponse<PageResult<BlackListAppDto>>(mapper.Map<PageResult<BlackListAppDto>>(await countryBll.GetAllAsync(searchParameters)));
    }
}
