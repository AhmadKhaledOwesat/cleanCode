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
    public class UserGroupController(IUserGroupBll UserGroupBll,IDcpMapper mapper) : BaseController<UserGroup,UserGroupDto,Guid,UserGroupFilter>(UserGroupBll, mapper)
    {
        public override async Task<DcpResponse<PageResult<UserGroupDto>>> GetAllAsync([FromBody] UserGroupFilter searchParameters)
        {
            return  new DcpResponse<PageResult<UserGroupDto>>(mapper.Map<PageResult<UserGroupDto>>(await UserGroupBll.GetAllAsync(searchParameters)));      
        }
    }
}
