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
    public class DeviceBatteryTransController(IDeviceBatteryTransBll DeviceBatteryTransBll, IDcpMapper mapper) : BaseController<DeviceBatteryTrans, DeviceBatteryTransDto, Guid, DeviceBatteryTransFilter>(DeviceBatteryTransBll, mapper)
    {
        public override async Task<DcpResponse<PageResult<DeviceBatteryTransDto>>> GetAllAsync([FromBody] DeviceBatteryTransFilter searchParameters)
        {
            return new DcpResponse<PageResult<DeviceBatteryTransDto>>(mapper.Map<PageResult<DeviceBatteryTransDto>>(await DeviceBatteryTransBll.GetAllAsync(searchParameters)));
        }
    }
}
