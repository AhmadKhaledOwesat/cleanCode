using MobCentra.Application.Bll;
using MobCentra.Application.Dto;
using MobCentra.Domain.Entities;
using MobCentra.Domain.Entities.Filters;
using MobCentra.Domain.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Profile = MobCentra.Domain.Entities.Profile;

namespace MobCentra.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowAllOrigins")]
    public class ProfileController(IProfileBll ProfileBll,IDcpMapper mapper) : BaseController<Profile,ProfileDto,Guid,ProfileFilter>(ProfileBll, mapper)
    {
        public override async Task<DcpResponse<PageResult<ProfileDto>>> GetAllAsync([FromBody] ProfileFilter searchParameters)
        {
            return  new DcpResponse<PageResult<ProfileDto>>(mapper.Map<PageResult<ProfileDto>>(await ProfileBll.GetAllAsync(searchParameters)));      
        }
        [HttpGet]
        [Route("device/{id}")]
        public async Task<DcpResponse<ProfileDto>> GetProfileByDeviceIdAsync(Guid id) => new DcpResponse<ProfileDto>(mapper.Map<ProfileDto>(await ProfileBll.GetProfileByDeviceIdAsync(id)));

    }
}
