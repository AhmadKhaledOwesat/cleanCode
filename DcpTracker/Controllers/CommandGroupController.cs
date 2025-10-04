using MobCentra.Application.Dto;
using MobCentra.Domain.Entities;
using MobCentra.Domain.Entities.Filters;
using MobCentra.Domain.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using CommandGroup = MobCentra.Domain.Entities.CommandGroup;

namespace MobCentra.Controllers
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
