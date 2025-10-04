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
    public class ApplicationsController(IApplicationBll ApplicationBll,IDcpMapper mapper) : BaseController<Domain.Entities.Application, ApplicationDto,Guid,ApplicationFilter>(ApplicationBll, mapper)
    {
        public override async Task<DcpResponse<PageResult<ApplicationDto>>> GetAllAsync([FromBody] ApplicationFilter searchParameters)
        {
            return  new DcpResponse<PageResult<ApplicationDto>>(mapper.Map<PageResult<ApplicationDto>>(await ApplicationBll.GetAllAsync(searchParameters)));      
        }
    }
}
