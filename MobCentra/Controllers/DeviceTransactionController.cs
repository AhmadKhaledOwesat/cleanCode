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
    public class DeviceTransactionController(IDeviceTransactionBll cityBll, IDcpMapper mapper) : BaseController<DeviceTransaction, DeviceTransactionDto, Guid, DeviceTransactionFilter>(cityBll, mapper)
    {
        public override async Task<DcpResponse<PageResult<DeviceTransactionDto>>> GetAllAsync([FromBody] DeviceTransactionFilter searchParameters)=> new DcpResponse<PageResult<DeviceTransactionDto>>(mapper.Map<PageResult<DeviceTransactionDto>>(await cityBll.GetAllAsync(searchParameters)));
    }
}
