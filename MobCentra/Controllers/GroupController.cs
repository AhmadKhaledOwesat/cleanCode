using MobCentra.Application.Dto;
using MobCentra.Domain.Entities;
using MobCentra.Domain.Entities.Filters;
using MobCentra.Domain.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Group = MobCentra.Domain.Entities.Group;

namespace MobCentra.Controllers
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
