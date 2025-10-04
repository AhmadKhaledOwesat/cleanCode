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
    public class StatisticController(IStatisticBll StatisticBll, IDcpMapper mapper) : BaseController<Statistic, StatisticDto, Guid, StatisticFilter>(StatisticBll, mapper)
    {
        public override async Task<DcpResponse<PageResult<StatisticDto>>> GetAllAsync([FromBody] StatisticFilter searchParameters) => new DcpResponse<PageResult<StatisticDto>>(mapper.Map<PageResult<StatisticDto>>(await StatisticBll.GetAllAsync(searchParameters)));
    }
}
