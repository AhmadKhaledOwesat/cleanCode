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
    public class UserCommandController(IUserCommandBll UserCommandBll,IDcpMapper mapper) : BaseController<UserCommand,UserCommandDto,Guid,UserCommandFilter>(UserCommandBll, mapper)
    {
        public override async Task<DcpResponse<PageResult<UserCommandDto>>> GetAllAsync([FromBody] UserCommandFilter searchParameters)
        {
            return  new DcpResponse<PageResult<UserCommandDto>>(mapper.Map<PageResult<UserCommandDto>>(await UserCommandBll.GetAllAsync(searchParameters)));      
        }
    }
}
