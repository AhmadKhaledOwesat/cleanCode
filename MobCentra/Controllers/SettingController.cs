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
    public class SettingController(ISettingBll settingBll, IDcpMapper mapper) : BaseController<Setting, SettingDto, Guid, SettingFilter>(settingBll, mapper)
    {
        public override async Task<DcpResponse<PageResult<SettingDto>>> GetAllAsync([FromBody] SettingFilter searchParameters)=> new DcpResponse<PageResult<SettingDto>>(mapper.Map<PageResult<SettingDto>>(await settingBll.GetAllAsync(searchParameters)));

        [HttpGet]
        [Route("setting/{deviceCode}/{settingName}")]
        public async Task<DcpResponse<PageResult<SettingDto>>> GetSettingsAsync([FromRoute] string deviceCode, [FromRoute] string settingName)
        {
            return new DcpResponse<PageResult<SettingDto>>(mapper.Map<PageResult<SettingDto>>( await settingBll.GetSettingsAsync(deviceCode, settingName)));
        }

    
    }
}
