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
    public class DeviceFileController(IDeviceFileBll DeviceFileBll, IDcpMapper mapper) : BaseController<DeviceFile, DeviceFileDto, Guid, DeviceFileFilter>(DeviceFileBll, mapper)
    {
        public override async Task<DcpResponse<PageResult<DeviceFileDto>>> GetAllAsync([FromBody] DeviceFileFilter searchParameters) => new DcpResponse<PageResult<DeviceFileDto>>(mapper.Map<PageResult<DeviceFileDto>>(await DeviceFileBll.GetAllAsync(searchParameters)));
    }
}
