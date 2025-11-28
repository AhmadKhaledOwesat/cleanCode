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
    public class NotificationController(INotificationBll NotificationBll,IDcpMapper mapper) : BaseController<Notifications,NotificationDto,Guid,NotificationFilter>(NotificationBll, mapper)
    {
        [HttpPost]
        [Route("search")]
        public override async Task<DcpResponse<PageResult<NotificationDto>>> GetAllAsync([FromBody] NotificationFilter searchParameters)
        {
            return  new DcpResponse<PageResult<NotificationDto>>(mapper.Map<PageResult<NotificationDto>>(await NotificationBll.GetAllAsync(searchParameters)));      
        }
    }
}
