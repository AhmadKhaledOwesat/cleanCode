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
    public class TasksController(ITasksBll cityBll, IDcpMapper mapper) : BaseController<Tasks, TasksDto, Guid, TaskFilter>(cityBll, mapper)
    {
        public override async Task<DcpResponse<PageResult<TasksDto>>> GetAllAsync([FromBody] TaskFilter searchParameters)=> new DcpResponse<PageResult<TasksDto>>(mapper.Map<PageResult<TasksDto>>(await cityBll.GetAllAsync(searchParameters)));
    }
}
