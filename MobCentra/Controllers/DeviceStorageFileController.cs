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
    public class DeviceStorageFileController(IDeviceStorageFileBll DeviceStorageFileBll, IDcpMapper mapper) : BaseController<DeviceStorageFile, DeviceStorageFileDto, Guid, DeviceStorageFileFilter>(DeviceStorageFileBll, mapper)
    {
        public override async Task<DcpResponse<PageResult<DeviceStorageFileDto>>> GetAllAsync([FromBody] DeviceStorageFileFilter searchParameters) => new DcpResponse<PageResult<DeviceStorageFileDto>>(mapper.Map<PageResult<DeviceStorageFileDto>>(await DeviceStorageFileBll.GetAllAsync(searchParameters)));
    }
}
