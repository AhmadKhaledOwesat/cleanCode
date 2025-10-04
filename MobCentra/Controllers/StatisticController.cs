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
    public class StatisticController(IStatisticBll StatisticBll, IDcpMapper mapper) : BaseController<Statistic, StatisticDto, Guid, StatisticFilter>(StatisticBll, mapper)
    {
        public override async Task<DcpResponse<PageResult<StatisticDto>>> GetAllAsync([FromBody] StatisticFilter searchParameters) => new DcpResponse<PageResult<StatisticDto>>(mapper.Map<PageResult<StatisticDto>>(await StatisticBll.GetAllAsync(searchParameters)));
    }
}
