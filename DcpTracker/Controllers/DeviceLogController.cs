using DcpTracker.Application.Dto;
using DcpTracker.Domain.Entities;
using DcpTracker.Domain.Entities.Filters;
using DcpTracker.Domain.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using DeviceLog = DcpTracker.Domain.Entities.DeviceLog;

namespace DcpTracker.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowAllOrigins")]
    public class DeviceLogController(IDeviceLogBll DeviceLogBll,IDcpMapper mapper) : BaseController<DeviceLog,DeviceLogDto,Guid,DeviceLogFilter>(DeviceLogBll, mapper)
    {
        public override async Task<DcpResponse<PageResult<DeviceLogDto>>> GetAllAsync([FromBody] DeviceLogFilter searchParameters)
        {
            return  new DcpResponse<PageResult<DeviceLogDto>>(mapper.Map<PageResult<DeviceLogDto>>(await DeviceLogBll.GetAllAsync(searchParameters)));      
        }
    }
}
