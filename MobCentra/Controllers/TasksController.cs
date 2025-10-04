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
    public class TasksController(ITasksBll cityBll, IDcpMapper mapper) : BaseController<Tasks, TasksDto, Guid, TaskFilter>(cityBll, mapper)
    {
        public override async Task<DcpResponse<PageResult<TasksDto>>> GetAllAsync([FromBody] TaskFilter searchParameters)=> new DcpResponse<PageResult<TasksDto>>(mapper.Map<PageResult<TasksDto>>(await cityBll.GetAllAsync(searchParameters)));
    }
}
