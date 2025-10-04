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
    public class DeviceUsageController(IDeviceUsageBll DeviceUsageBll, IDcpMapper mapper) : BaseController<DeviceUsage, DeviceUsageDto, Guid, DeviceUsageFilter>(DeviceUsageBll, mapper)
    {
        public override async Task<DcpResponse<PageResult<DeviceUsageDto>>> GetAllAsync([FromBody] DeviceUsageFilter searchParameters)
        {
            return new DcpResponse<PageResult<DeviceUsageDto>>(mapper.Map<PageResult<DeviceUsageDto>>(await DeviceUsageBll.GetAllAsync(searchParameters)));
        }
    }

}
