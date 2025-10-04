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
    public class ProfileFeatureController(IProfileFeatureBll ProfileFeatureBll,IDcpMapper mapper) : BaseController<ProfileFeature,ProfileFeatureDto,Guid,ProfileFeatureFilter>(ProfileFeatureBll, mapper)
    {
        public override async Task<DcpResponse<PageResult<ProfileFeatureDto>>> GetAllAsync([FromBody] ProfileFeatureFilter searchParameters)
        {
            return  new DcpResponse<PageResult<ProfileFeatureDto>>(mapper.Map<PageResult<ProfileFeatureDto>>(await ProfileFeatureBll.GetAllAsync(searchParameters)));      
        }
    }
}
