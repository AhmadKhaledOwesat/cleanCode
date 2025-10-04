using DcpTracker.Application.Dto;
using DcpTracker.Domain.Entities;
using DcpTracker.Domain.Entities.Filters;
using DcpTracker.Domain.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using DeviceNotification = DcpTracker.Domain.Entities.DeviceNotification;

namespace DcpTracker.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowAllOrigins")]
    public class DeviceNotificationController(IDeviceNotificationBll DeviceNotificationBll,IDcpMapper mapper) : BaseController<DeviceNotification,DeviceNotificationDto,Guid,DeviceNotificationFilter>(DeviceNotificationBll, mapper)
    {
        public override async Task<DcpResponse<PageResult<DeviceNotificationDto>>> GetAllAsync([FromBody] DeviceNotificationFilter searchParameters)
        {
            return  new DcpResponse<PageResult<DeviceNotificationDto>>(mapper.Map<PageResult<DeviceNotificationDto>>(await DeviceNotificationBll.GetAllAsync(searchParameters)));      
        }
    }
}
