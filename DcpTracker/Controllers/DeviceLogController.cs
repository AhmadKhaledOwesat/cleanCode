using MobCentra.Application.Dto;
using MobCentra.Domain.Entities;
using MobCentra.Domain.Entities.Filters;
using MobCentra.Domain.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using DeviceLog = MobCentra.Domain.Entities.DeviceLog;

namespace MobCentra.Controllers
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
