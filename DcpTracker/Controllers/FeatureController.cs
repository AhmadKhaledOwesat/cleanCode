using MobCentra.Application.Dto;
using MobCentra.Domain.Entities;
using MobCentra.Domain.Entities.Filters;
using MobCentra.Domain.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Feature = MobCentra.Domain.Entities.Feature;

namespace MobCentra.Controllers
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
