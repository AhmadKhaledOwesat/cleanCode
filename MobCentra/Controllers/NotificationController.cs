using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MobCentra.Application.Dto;
using MobCentra.Domain.Entities;
using MobCentra.Domain.Entities.Filters;
using MobCentra.Domain.Interfaces;

namespace MobCentra.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowAllOrigins")]
    public class NotificationController(INotificationBll notificationBll, IDcpMapper mapper) : BaseController<Notifications, NotificationDto, Guid, NotificationFilter>(notificationBll, mapper)
    {
        [HttpPost]
        [Route("search")]
        public override async Task<DcpResponse<PageResult<NotificationDto>>> GetAllAsync([FromBody] NotificationFilter searchParameters)
        {

            if (!await notificationBll.IsAuthorizedAsync(Guid.Parse(Permissions.Notifications)))
                throw new UnauthorizedAccessException();

            return new DcpResponse<PageResult<NotificationDto>>(mapper.Map<PageResult<NotificationDto>>(await notificationBll.GetAllAsync(searchParameters)));
        }
    }
}
