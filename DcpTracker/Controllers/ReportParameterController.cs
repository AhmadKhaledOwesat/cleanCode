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
    public class ReportParameterController(IReportParameterBll ReportParameterBll, IDcpMapper mapper) : BaseController<ReportParameter, ReportParameterDto, Guid, ReportParameterFilter>(ReportParameterBll, mapper)
    {
        public override async Task<DcpResponse<PageResult<ReportParameterDto>>> GetAllAsync([FromBody] ReportParameterFilter searchParameters) => new DcpResponse<PageResult<ReportParameterDto>>(mapper.Map<PageResult<ReportParameterDto>>(await ReportParameterBll.GetAllAsync(searchParameters)));
    }
}
