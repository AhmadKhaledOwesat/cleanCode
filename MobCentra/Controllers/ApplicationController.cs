using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MobCentra.Application.Dto;
using MobCentra.Domain.Entities;
using MobCentra.Domain.Entities.Filters;
using MobCentra.Domain.Interfaces;

namespace MobCentra.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowAllOrigins")]
    public class ApplicationsController(IApplicationBll applicationBll, IDcpMapper mapper) : BaseController<Domain.Entities.Application, ApplicationDto, Guid, ApplicationFilter>(applicationBll, mapper)
    {
        public override async Task<DcpResponse<PageResult<ApplicationDto>>> GetAllAsync([FromBody] ApplicationFilter searchParameters)
        {
            if (!await applicationBll.IsAuthorizedAsync(Guid.Parse(Permissions.Apks)))
                throw new UnauthorizedAccessException();

            return new DcpResponse<PageResult<ApplicationDto>>(mapper.Map<PageResult<ApplicationDto>>(await applicationBll.GetAllAsync(searchParameters)));
        }
    }
}
