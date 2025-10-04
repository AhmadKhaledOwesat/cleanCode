using MobCentra.Application.Dto;
using MobCentra.Application.Interfaces;
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
    public class UserController(IUserBll userBll,IDcpMapper mapper) : BaseController<Users,UsersDto,Guid,UserFilter>(userBll, mapper)
    {
        public override async Task<DcpResponse<PageResult<UsersDto>>> GetAllAsync([FromBody] UserFilter searchParameters)=> new DcpResponse<PageResult<UsersDto>>(mapper.Map<PageResult<UsersDto>>(await userBll.GetAllAsync(searchParameters)));
        [HttpGet]
        [Route("login/{userName}/{password}/{companyCode}")]
        public async Task<DcpResponse<UsersDto>> LoginAsync(string userName, string password, string companyCode) => await userBll.LoginAsync(userName,  password, companyCode);

        [HttpGet]
        [Route("reset-password/{userName}/{companyCode}")]
        public async Task<DcpResponse<string>> ResetPasswordAsync(string userName, string companyCode) => await userBll.ResetPasswordAsync(userName, companyCode);

        [HttpGet]
        [Route("update-password/{userId}/{newPassword}")]
        public async Task<DcpResponse<string>> UpdatePasswordAsync(Guid userId, string newPassword) => await userBll.UpdatePasswordAsync(userId, newPassword);
    }
}
