using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MobCentra.Application.Dto;
using MobCentra.Application.Interfaces;
using MobCentra.Domain.Entities;
using MobCentra.Domain.Entities.Filters;
using MobCentra.Domain.Interfaces;

namespace MobCentra.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowAllOrigins")]
    public class EmailLogController(IEmailLogBll emailLogBll, IDcpMapper mapper) : BaseController<EmailLog, EmailLogDto, Guid, EmailLogFilter>(emailLogBll, mapper)
    {
        public override async Task<DcpResponse<PageResult<EmailLogDto>>> GetAllAsync([FromBody] EmailLogFilter searchParameters)
        {
            if (!searchParameters.IsByPass && !await emailLogBll.IsAuthorizedAsync(Guid.Parse(Permissions.Report)))
                throw new UnauthorizedAccessException();
            return new DcpResponse<PageResult<EmailLogDto>>(mapper.Map<PageResult<EmailLogDto>>(await emailLogBll.GetAllAsync(searchParameters)));
        }
    }
}
