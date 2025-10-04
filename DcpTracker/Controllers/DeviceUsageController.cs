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
    public class DeviceUsageController(IDeviceUsageBll DeviceUsageBll, IDcpMapper mapper) : BaseController<DeviceUsage, DeviceUsageDto, Guid, DeviceUsageFilter>(DeviceUsageBll, mapper)
    {
        public override async Task<DcpResponse<PageResult<DeviceUsageDto>>> GetAllAsync([FromBody] DeviceUsageFilter searchParameters)
        {
            return new DcpResponse<PageResult<DeviceUsageDto>>(mapper.Map<PageResult<DeviceUsageDto>>(await DeviceUsageBll.GetAllAsync(searchParameters)));
        }
    }

}
