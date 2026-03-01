using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MobCentra.Application.Dto;
using MobCentra.Domain.Entities;
using MobCentra.Domain.Entities.Filters;
using MobCentra.Domain.Interfaces;
using Device = MobCentra.Domain.Entities.Device;

namespace MobCentra.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowAllOrigins")]
    public class DeviceController(IDeviceBll deviceBll, IDcpMapper mapper) : BaseController<Device, DeviceDto, Guid, DeviceFilter>(deviceBll, mapper)
    {
        public override async Task<DcpResponse<PageResult<DeviceDto>>> GetAllAsync([FromBody] DeviceFilter searchParameters)
        {
            if (!await deviceBll.IsAuthorizedAsync(Guid.Parse(Permissions.Devices)))
                throw new UnauthorizedAccessException();
            try
            {
                return new DcpResponse<PageResult<DeviceDto>>(mapper.Map<PageResult<DeviceDto>>(await deviceBll.GetAllAsync(searchParameters)));

            }
            catch (Exception ex)
            {

            }
            return null;
        }
        [HttpPost]
        [Route("sendCommand")]
        public async Task<DcpResponse<string>> SendCommandAsync([FromBody] SendCommandDto sendCommandDto) => await deviceBll.SendCommandAsync(sendCommandDto);
        [HttpPost]
        [Route("sendNotify")]
        public async Task<DcpResponse<string>> SendNotifyAsync([FromBody] SendNotifyDto sendNotifyDto) => await deviceBll.SendNotifyAsync(sendNotifyDto);
        [HttpGet]
        [Route("status")]
        public async Task UpdateDeviceStatusAsync()
        {
            await Task.FromResult(Task.CompletedTask);
        }

        [HttpGet]
        [Route("{fromDate}/{toDate}")]
        public async Task<DcpResponse<string>> DeleteRecordAsync(DateTime? fromDate, DateTime? toDate) => await deviceBll.DeleteRecordAsync(fromDate, toDate);

        [HttpGet]
        [Route("refresh/{companyId}")]
        public async Task<DcpResponse<string>> UpdateAppAsync(Guid companyId) => await deviceBll.SendCommandAsync(new SendCommandDto { Command = "silent_install", CompanyId = companyId, IgnoreExecption = true, IsInternal = true, Token = [] });

        [HttpGet]
        [Route("check/{companyId}")]
        public async Task<DcpResponse<object>> CheckSettingsAsync(Guid companyId)
        {
            if (!await deviceBll.IsAuthorizedAsync(Guid.Parse(Permissions.QrCode)))
                throw new UnauthorizedAccessException();

           return await deviceBll.CheckSettingsAsync(companyId);
        }
        [HttpGet]
        [Route("getVersionCount/{companyId}")]
        public async Task<DcpResponse<dynamic>> GetVersionCountAsync(Guid companyId)
        {
            return await deviceBll.GetVersionCountAsync(companyId);
        }

        [HttpPost]
        [Route("uploadImage")]
        public async Task<DcpResponse<bool>> UploadImageAsync([FromBody] ImageDto imageDto) => await deviceBll.UploadImageAndSendCommandAsync(imageDto);
        [HttpPost]
        [Route("uploadFile")]
        public async Task<DcpResponse<bool>> UploadFileAsync([FromBody] ImageDto imageDto)
        {
            return await deviceBll.UploadFileAndSendCommandAsync(imageDto);
        }


    }

}
