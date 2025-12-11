using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MobCentra.Application.Dto;
using MobCentra.Domain.Entities;
using MobCentra.Domain.Entities.Filters;
using MobCentra.Domain.Interfaces;
using Profile = MobCentra.Domain.Entities.Profile;

namespace MobCentra.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowAllOrigins")]
    public class ProfileController(IProfileBll profileBll, IDcpMapper mapper) : BaseController<Profile, ProfileDto, Guid, ProfileFilter>(profileBll, mapper)
    {
        public override async Task<DcpResponse<PageResult<ProfileDto>>> GetAllAsync([FromBody] ProfileFilter searchParameters)
        {
            if (!searchParameters.IsByPass && !await profileBll.IsAuthorizedAsync(Guid.Parse(Permissions.Profile)))
                throw new UnauthorizedAccessException();

            return new DcpResponse<PageResult<ProfileDto>>(mapper.Map<PageResult<ProfileDto>>(await profileBll.GetAllAsync(searchParameters)));
        }
        [HttpGet]
        [Route("device/{id}")]
        public async Task<DcpResponse<ProfileDto>> GetProfileByDeviceIdAsync(Guid id) => new DcpResponse<ProfileDto>(mapper.Map<ProfileDto>(await profileBll.GetProfileByDeviceIdAsync(id)));

    }
}
