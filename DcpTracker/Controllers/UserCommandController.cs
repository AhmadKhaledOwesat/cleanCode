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
    public class UserCommandController(IUserCommandBll UserCommandBll,IDcpMapper mapper) : BaseController<UserCommand,UserCommandDto,Guid,UserCommandFilter>(UserCommandBll, mapper)
    {
        public override async Task<DcpResponse<PageResult<UserCommandDto>>> GetAllAsync([FromBody] UserCommandFilter searchParameters)
        {
            return  new DcpResponse<PageResult<UserCommandDto>>(mapper.Map<PageResult<UserCommandDto>>(await UserCommandBll.GetAllAsync(searchParameters)));      
        }
    }
}
