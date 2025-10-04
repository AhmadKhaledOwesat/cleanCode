using DcpTracker.Application.Dto;
using DcpTracker.Domain.Entities;
using DcpTracker.Domain.Entities.Filters;
using DcpTracker.Domain.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using City = DcpTracker.Domain.Entities.City;

namespace DcpTracker.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowAllOrigins")]
    public class CityController(ICityBll CityBll,IDcpMapper mapper) : BaseController<City,CityDto,Guid,CityFilter>(CityBll, mapper)
    {
        public override async Task<DcpResponse<PageResult<CityDto>>> GetAllAsync([FromBody] CityFilter searchParameters)
        {
            return  new DcpResponse<PageResult<CityDto>>(mapper.Map<PageResult<CityDto>>(await CityBll.GetAllAsync(searchParameters)));      
        }
    }
}
