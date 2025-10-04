using DcpTracker.Application.Dto;
using DcpTracker.Domain.Entities;
using DcpTracker.Domain.Entities.Filters;
using DcpTracker.Domain.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using CommandGroup = DcpTracker.Domain.Entities.CommandGroup;

namespace DcpTracker.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowAllOrigins")]
    public class CommandGroupController(ICommandGroupBll CommandGroupBll,IDcpMapper mapper) : BaseController<CommandGroup,CommandGroupDto,Guid,CommandGroupFilter>(CommandGroupBll, mapper)
    {
        public override async Task<DcpResponse<PageResult<CommandGroupDto>>> GetAllAsync([FromBody] CommandGroupFilter searchParameters)
        {
            return  new DcpResponse<PageResult<CommandGroupDto>>(mapper.Map<PageResult<CommandGroupDto>>(await CommandGroupBll.GetAllAsync(searchParameters)));      
        }
    }
}
