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
    public class ProfileFeatureController(IProfileFeatureBll ProfileFeatureBll,IDcpMapper mapper) : BaseController<ProfileFeature,ProfileFeatureDto,Guid,ProfileFeatureFilter>(ProfileFeatureBll, mapper)
    {
        public override async Task<DcpResponse<PageResult<ProfileFeatureDto>>> GetAllAsync([FromBody] ProfileFeatureFilter searchParameters)
        {
            return  new DcpResponse<PageResult<ProfileFeatureDto>>(mapper.Map<PageResult<ProfileFeatureDto>>(await ProfileFeatureBll.GetAllAsync(searchParameters)));      
        }
    }
}
