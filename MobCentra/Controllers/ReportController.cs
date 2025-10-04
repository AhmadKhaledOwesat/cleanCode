using MobCentra.Application.Dto;
using MobCentra.Application.Interfaces;
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
    public class ReportController(IReportBll ReportBll, IDcpMapper mapper) : BaseController<Report, ReportDto, Guid, ReportFilter>(ReportBll, mapper)
    {
        public override async Task<DcpResponse<PageResult<ReportDto>>> GetAllAsync([FromBody] ReportFilter searchParameters)=> new DcpResponse<PageResult<ReportDto>>(mapper.Map<PageResult<ReportDto>>(await ReportBll.GetAllAsync(searchParameters)));
       
        [HttpGet]
        [Route("excute/{query}")]
        public  async Task<DcpResponse<dynamic>> ExecuteReportAsync([FromRoute] string query) => await ReportBll.ExecuteReport(query);

    }
}
