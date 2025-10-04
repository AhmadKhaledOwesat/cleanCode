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
    public class BlackListAppController(IBlackListAppBll countryBll, IDcpMapper mapper) : BaseController<BlackListApp, BlackListAppDto, Guid, BlackListAppFilter>(countryBll, mapper)
    {
        public override async Task<DcpResponse<PageResult<BlackListAppDto>>> GetAllAsync([FromBody] BlackListAppFilter searchParameters)=> new DcpResponse<PageResult<BlackListAppDto>>(mapper.Map<PageResult<BlackListAppDto>>(await countryBll.GetAllAsync(searchParameters)));
    }
}
