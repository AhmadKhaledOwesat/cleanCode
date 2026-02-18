using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MobCentra.Application.Dto;
using MobCentra.Application.Interfaces;
using MobCentra.Domain.Entities;
using MobCentra.Domain.Entities.Filters;
using MobCentra.Domain.Interfaces;

namespace MobCentra.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowAllOrigins")]
    public class UserController(IUserBll userBll, IDcpMapper mapper, IIdentityManager<Guid> identityManager, IAuthenticationManager authenticationManager) : BaseController<Users, UsersDto, Guid, UserFilter>(userBll, mapper)
    {
        public override async Task<DcpResponse<PageResult<UsersDto>>> GetAllAsync([FromBody] UserFilter searchParameters)
        {
            if (!searchParameters.IsByPass && !await userBll.IsAuthorizedAsync(Guid.Parse(Permissions.Users)))
                throw new UnauthorizedAccessException();

            return new DcpResponse<PageResult<UsersDto>>(mapper.Map<PageResult<UsersDto>>(await userBll.GetAllAsync(searchParameters)));
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<DcpResponse<UsersDto>> LoginAsync([FromBody] UserLoginRequestDto request) => await userBll.LoginAsync(request.UserName, request.Password, request.CompanyCode);

        [HttpPost]
        [Route("reset-password")]
        [AllowAnonymous]
        public async Task<DcpResponse<string>> ResetPasswordAsync([FromBody] ResetPasswordRequestDto request) => await userBll.ResetPasswordAsync(request.UserName, request.CompanyCode);

        [HttpPost]
        [Route("update-password")]
        public async Task<DcpResponse<string>> UpdatePasswordAsync([FromBody] UpdatePasswordRequestDto request) => await userBll.UpdatePasswordAsync(identityManager.CurrentUserId, request.UserId, request.NewPassword);

        [HttpPost]
        [Route("refresh-token")]
        [AllowAnonymous]
        public async Task<ActionResult<DcpResponse<Tokens>>> RefreshTokenAsync()
        {
            var bearerToken = Request.Headers.Authorization.FirstOrDefault();
            if (string.IsNullOrEmpty(bearerToken))
                return Unauthorized(new DcpResponse<Tokens>(default!, "Authorization header is required. Send the refresh token (refreshToken from login), not the access token.", false));
            try
            {
                var tokens = await Task.Run(() => authenticationManager.RefreshToken(bearerToken));
                return Ok(new DcpResponse<Tokens>(tokens));
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new DcpResponse<Tokens>(default!, ex.Message, false));
            }
        }
    }
}
