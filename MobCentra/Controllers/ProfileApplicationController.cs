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
    public class ProfileApplicationController(IProfileApplicationBll ProfileApplicationBll,IDcpMapper mapper) : BaseController<ProfileApplication,ProfileApplicationDto,Guid,ProfileApplicationFilter>(ProfileApplicationBll, mapper)
    {
        public override async Task<DcpResponse<PageResult<ProfileApplicationDto>>> GetAllAsync([FromBody] ProfileApplicationFilter searchParameters)
        {
            return  new DcpResponse<PageResult<ProfileApplicationDto>>(mapper.Map<PageResult<ProfileApplicationDto>>(await ProfileApplicationBll.GetAllAsync(searchParameters)));      
        }
    }
}
