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
    public class GeoFencController(IGeoFencBll GeoFencBll,IDcpMapper mapper) : BaseController<GeoFenc, GeoFencDto,Guid,GeoFencFilter>(GeoFencBll, mapper)
    {
        public override async Task<DcpResponse<PageResult<GeoFencDto>>> GetAllAsync([FromBody] GeoFencFilter searchParameters)
        {
            return  new DcpResponse<PageResult<GeoFencDto>>(mapper.Map<PageResult<GeoFencDto>>(await GeoFencBll.GetAllAsync(searchParameters)));      
        }


    }
}
