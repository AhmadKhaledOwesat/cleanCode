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
    public class GeoFencController(IGeoFencBll GeoFencBll,IDcpMapper mapper) : BaseController<GeoFenc, GeoFencDto,Guid,GeoFencFilter>(GeoFencBll, mapper)
    {
        public override async Task<DcpResponse<PageResult<GeoFencDto>>> GetAllAsync([FromBody] GeoFencFilter searchParameters)
        {
            return  new DcpResponse<PageResult<GeoFencDto>>(mapper.Map<PageResult<GeoFencDto>>(await GeoFencBll.GetAllAsync(searchParameters)));      
        }


    }
}
