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
    public class DeviceBatteryTransController(IDeviceBatteryTransBll DeviceBatteryTransBll, IDcpMapper mapper) : BaseController<DeviceBatteryTrans, DeviceBatteryTransDto, Guid, DeviceBatteryTransFilter>(DeviceBatteryTransBll, mapper)
    {
        public override async Task<DcpResponse<PageResult<DeviceBatteryTransDto>>> GetAllAsync([FromBody] DeviceBatteryTransFilter searchParameters)
        {
            return new DcpResponse<PageResult<DeviceBatteryTransDto>>(mapper.Map<PageResult<DeviceBatteryTransDto>>(await DeviceBatteryTransBll.GetAllAsync(searchParameters)));
        }
    }
}
