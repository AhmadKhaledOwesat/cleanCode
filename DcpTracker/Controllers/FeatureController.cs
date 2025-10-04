using DcpTracker.Application.Dto;
using DcpTracker.Domain.Entities;
using DcpTracker.Domain.Entities.Filters;
using DcpTracker.Domain.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Feature = DcpTracker.Domain.Entities.Feature;

namespace DcpTracker.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowAllOrigins")]
    public class FeatureController(IFeatureBll FeatureBll,IDcpMapper mapper) : BaseController<Feature,FeatureDto,int,FeatureFilter>(FeatureBll, mapper)
    {
        public override async Task<DcpResponse<PageResult<FeatureDto>>> GetAllAsync([FromBody] FeatureFilter searchParameters)
        {
            return  new DcpResponse<PageResult<FeatureDto>>(mapper.Map<PageResult<FeatureDto>>(await FeatureBll.GetAllAsync(searchParameters)));      
        }
    }
}
