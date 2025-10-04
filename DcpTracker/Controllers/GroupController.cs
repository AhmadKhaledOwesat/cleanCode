using DcpTracker.Application.Dto;
using DcpTracker.Domain.Entities;
using DcpTracker.Domain.Entities.Filters;
using DcpTracker.Domain.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Group = DcpTracker.Domain.Entities.Group;

namespace DcpTracker.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowAllOrigins")]
    public class GroupController(IGroupBll GroupBll,IDcpMapper mapper) : BaseController<Group,GroupDto,Guid,GroupFilter>(GroupBll, mapper)
    {
        public override async Task<DcpResponse<PageResult<GroupDto>>> GetAllAsync([FromBody] GroupFilter searchParameters)
        {
            return  new DcpResponse<PageResult<GroupDto>>(mapper.Map<PageResult<GroupDto>>(await GroupBll.GetAllAsync(searchParameters)));      
        }
    }
}
