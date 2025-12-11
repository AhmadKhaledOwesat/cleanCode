using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MobCentra.Application.Dto;
using MobCentra.Domain.Entities;
using MobCentra.Domain.Entities.Filters;
using MobCentra.Domain.Interfaces;

namespace MobCentra.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowAllOrigins")]
    public class CityController(ICityBll cityBll, IDcpMapper mapper) : BaseController<City, CityDto, Guid, CityFilter>(cityBll, mapper)
    {
        public override async Task<DcpResponse<PageResult<CityDto>>> GetAllAsync([FromBody] CityFilter searchParameters)
        {
            if (!searchParameters.IsByPass && !await cityBll.IsAuthorizedAsync(Guid.Parse(Permissions.City)))
                throw new UnauthorizedAccessException();

            return new DcpResponse<PageResult<CityDto>>(mapper.Map<PageResult<CityDto>>(await cityBll.GetAllAsync(searchParameters)));
        }
    }
}
