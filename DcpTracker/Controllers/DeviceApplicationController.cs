using MobCentra.Application.Dto;
using MobCentra.Domain.Entities;
using MobCentra.Domain.Entities.Filters;
using MobCentra.Domain.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using DeviceApplication = MobCentra.Domain.Entities.DeviceApplication;

namespace MobCentra.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowAllOrigins")]
    public class DeviceApplicationController(IDeviceApplicationBll DeviceApplicationBll, IDcpMapper mapper) : BaseController<DeviceApplication, DeviceApplicationDto, Guid, DeviceApplicationFilter>(DeviceApplicationBll, mapper)
    {
        public override async Task<DcpResponse<PageResult<DeviceApplicationDto>>> GetAllAsync([FromBody] DeviceApplicationFilter searchParameters)
        {
            return new DcpResponse<PageResult<DeviceApplicationDto>>(mapper.Map<PageResult<DeviceApplicationDto>>(await DeviceApplicationBll.GetAllAsync(searchParameters)));
        }
        [HttpPost]
        [Route("blocked")]
        public async Task<DcpResponse<bool>> UpdateStatusAsync([FromBody] DeviceBlockedApplicationDto[] deviceBlockedApplicationDto) => new DcpResponse<bool>(await DeviceApplicationBll.UpdateStatus(deviceBlockedApplicationDto));
    }
}
