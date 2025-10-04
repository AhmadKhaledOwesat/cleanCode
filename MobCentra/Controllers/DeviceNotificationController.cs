using MobCentra.Application.Dto;
using MobCentra.Domain.Entities;
using MobCentra.Domain.Entities.Filters;
using MobCentra.Domain.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using DeviceNotification = MobCentra.Domain.Entities.DeviceNotification;

namespace MobCentra.Controllers
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
