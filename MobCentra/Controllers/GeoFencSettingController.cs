using MobCentra.Application.Dto;
using MobCentra.Domain.Entities;
using MobCentra.Domain.Entities.Filters;
using MobCentra.Domain.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MobCentra.Application.Interfaces;

namespace MobCentra.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowAllOrigins")]
    public class GeoFencSettingController(IGeoFencSettingBll geoFencSettingBll , IDcpMapper mapper) : BaseController<GeoFencSetting, GeoFencSettingDto, Guid, GeoFencSettingFilter>(geoFencSettingBll, mapper)
    {
        public override async Task<DcpResponse<PageResult<GeoFencSettingDto>>> GetAllAsync([FromBody] GeoFencSettingFilter searchParameters) => new DcpResponse<PageResult<GeoFencSettingDto>>(mapper.Map<PageResult<GeoFencSettingDto>>(await geoFencSettingBll.GetAllAsync(searchParameters)));
    }
}
