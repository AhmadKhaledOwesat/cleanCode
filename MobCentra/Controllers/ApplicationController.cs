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
    public class ApplicationsController(IApplicationBll ApplicationBll,IDcpMapper mapper) : BaseController<Domain.Entities.Application, ApplicationDto,Guid,ApplicationFilter>(ApplicationBll, mapper)
    {
        public override async Task<DcpResponse<PageResult<ApplicationDto>>> GetAllAsync([FromBody] ApplicationFilter searchParameters)
        {
            return  new DcpResponse<PageResult<ApplicationDto>>(mapper.Map<PageResult<ApplicationDto>>(await ApplicationBll.GetAllAsync(searchParameters)));      
        }
    }
}
