using DcpTracker.Application.Bll;
using DcpTracker.Application.Dto;
using DcpTracker.Domain.Entities;
using DcpTracker.Domain.Entities.Filters;
using DcpTracker.Domain.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Profile = DcpTracker.Domain.Entities.Profile;

namespace DcpTracker.Controllers
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
