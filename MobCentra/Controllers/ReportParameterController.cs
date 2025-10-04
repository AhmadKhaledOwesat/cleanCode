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
    public class ReportParameterController(IReportParameterBll ReportParameterBll, IDcpMapper mapper) : BaseController<ReportParameter, ReportParameterDto, Guid, ReportParameterFilter>(ReportParameterBll, mapper)
    {
        public override async Task<DcpResponse<PageResult<ReportParameterDto>>> GetAllAsync([FromBody] ReportParameterFilter searchParameters) => new DcpResponse<PageResult<ReportParameterDto>>(mapper.Map<PageResult<ReportParameterDto>>(await ReportParameterBll.GetAllAsync(searchParameters)));
    }
}
