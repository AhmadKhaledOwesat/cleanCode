using MobCentra.Application.Dto;
using MobCentra.Domain.Entities;
using MobCentra.Domain.Entities.Filters;
using MobCentra.Domain.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using TaskStatus = MobCentra.Domain.Entities.TaskStatus;

namespace MobCentra.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowAllOrigins")]
    public class TaskStatusController(ITaskStatusBll TaskStatusBll,IDcpMapper mapper) : BaseController<TaskStatus,TaskStatusDto,Guid,TaskStatusFilter>(TaskStatusBll, mapper)
    {
        public override async Task<DcpResponse<PageResult<TaskStatusDto>>> GetAllAsync([FromBody] TaskStatusFilter searchParameters)
        {
            return  new DcpResponse<PageResult<TaskStatusDto>>(mapper.Map<PageResult<TaskStatusDto>>(await TaskStatusBll.GetAllAsync(searchParameters)));      
        }
    }
}
