using MobCentra.Application.Dto;
using MobCentra.Domain.Entities;
using MobCentra.Domain.Entities.Filters;
using MobCentra.Domain.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using City = MobCentra.Domain.Entities.City;

namespace MobCentra.Controllers
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
