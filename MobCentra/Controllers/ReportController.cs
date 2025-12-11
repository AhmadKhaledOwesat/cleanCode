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
    public class ReportController(IReportBll reportBll, IDcpMapper mapper) : BaseController<Report, ReportDto, Guid, ReportFilter>(reportBll, mapper)
    {
        public override async Task<DcpResponse<PageResult<ReportDto>>> GetAllAsync([FromBody] ReportFilter searchParameters)
        {

            if (!searchParameters.IsByPass && !await reportBll.IsAuthorizedAsync(Guid.Parse(Permissions.Report)))
                throw new UnauthorizedAccessException();

            return new DcpResponse<PageResult<ReportDto>>(mapper.Map<PageResult<ReportDto>>(await reportBll.GetAllAsync(searchParameters)));

        }

        [HttpGet]
        [Route("excute/{query}")]
        public async Task<DcpResponse<dynamic>> ExecuteReportAsync([FromRoute] string query) => await reportBll.ExecuteReport(query);

    }
}
