using Microsoft.AspNetCore.Authorization;
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
    public class CompanyController(ICompanyBll companyBll, IDcpMapper mapper) : BaseController<Company, CompanyDto, Guid, CompanyFilter>(companyBll, mapper)
    {
        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<DcpResponse<CompanyDto>> LoginAsync([FromBody] UserLoginRequestDto request) => await companyBll.LoginAsync(request.UserName, request.Password, request.CompanyCode);

        public override async Task<DcpResponse<PageResult<CompanyDto>>> GetAllAsync([FromBody] CompanyFilter searchParameters) => new DcpResponse<PageResult<CompanyDto>>(mapper.Map<PageResult<CompanyDto>>(await companyBll.GetAllAsync(searchParameters)));
    }
}
