using DcpTracker.Application.Dto;
using DcpTracker.Domain.Entities;
using DcpTracker.Domain.Entities.Filters;
using DcpTracker.Domain.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using TaskStatus = DcpTracker.Domain.Entities.TaskStatus;

namespace DcpTracker.Controllers
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
