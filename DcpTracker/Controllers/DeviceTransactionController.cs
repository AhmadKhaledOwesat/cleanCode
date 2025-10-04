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
    public class DeviceTransactionController(IDeviceTransactionBll cityBll, IDcpMapper mapper) : BaseController<DeviceTransaction, DeviceTransactionDto, Guid, DeviceTransactionFilter>(cityBll, mapper)
    {
        public override async Task<DcpResponse<PageResult<DeviceTransactionDto>>> GetAllAsync([FromBody] DeviceTransactionFilter searchParameters)=> new DcpResponse<PageResult<DeviceTransactionDto>>(mapper.Map<PageResult<DeviceTransactionDto>>(await cityBll.GetAllAsync(searchParameters)));
    }
}
