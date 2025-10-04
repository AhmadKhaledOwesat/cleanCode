using MobCentra.Application.Bll;
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
    public class CompanyController(ICompanyBll companyBll, IDcpMapper mapper) : BaseController<Company, CompanyDto, Guid, CompanyFilter>(companyBll, mapper)
    {
        [HttpGet]
        [Route("login/{userName}/{password}/{companyCode}")]
        public async Task<DcpResponse<CompanyDto>> LoginAsync(string userName, string password, string companyCode) => await companyBll.LoginAsync(userName, password, companyCode);

        public override async Task<DcpResponse<PageResult<CompanyDto>>> GetAllAsync([FromBody] CompanyFilter searchParameters)=> new DcpResponse<PageResult<CompanyDto>>(mapper.Map<PageResult<CompanyDto>>(await companyBll.GetAllAsync(searchParameters)));
    }
}
