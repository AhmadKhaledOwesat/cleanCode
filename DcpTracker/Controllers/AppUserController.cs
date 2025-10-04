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
    public class AppUserController(IAppUserBll appUserBll, IDcpMapper mapper) : BaseController<AppUsers, AppUserDto, Guid, AppUserFilter>(appUserBll, mapper)
    {
        public override async Task<DcpResponse<PageResult<AppUserDto>>> GetAllAsync([FromBody] AppUserFilter searchParameters) => new DcpResponse<PageResult<AppUserDto>>(mapper.Map<PageResult<AppUserDto>>(await appUserBll.GetAllAsync(searchParameters)));
        [HttpPost]
        [Route("login")]
        public async Task<DcpResponse<AppUserDto>> LoginAsync([FromBody] LoginDto loginDto) => await appUserBll.LoginAsync(loginDto);
    }
}
