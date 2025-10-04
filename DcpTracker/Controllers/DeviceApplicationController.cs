using DcpTracker.Application.Dto;
using DcpTracker.Domain.Entities;
using DcpTracker.Domain.Entities.Filters;
using DcpTracker.Domain.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using DeviceApplication = DcpTracker.Domain.Entities.DeviceApplication;

namespace DcpTracker.Controllers
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
