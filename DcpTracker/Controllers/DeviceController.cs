using DcpTracker.Application.Dto;
using DcpTracker.Domain.Entities;
using DcpTracker.Domain.Entities.Filters;
using DcpTracker.Domain.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Device = DcpTracker.Domain.Entities.Device;

namespace DcpTracker.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowAllOrigins")]
    public class DeviceController(IDeviceBll deviceBll,IDcpMapper mapper) : BaseController<Device,DeviceDto,Guid,DeviceFilter>(deviceBll, mapper)
    {
        public override async Task<DcpResponse<PageResult<DeviceDto>>> GetAllAsync([FromBody] DeviceFilter searchParameters)
        {
            return  new DcpResponse<PageResult<DeviceDto>>(mapper.Map<PageResult<DeviceDto>>(await deviceBll.GetAllAsync(searchParameters)));      
        }
        [HttpPost]
        [Route("sendCommand")]
        public async Task<DcpResponse<string>> SendCommandAsync([FromBody] SendCommandDto sendCommandDto)=> await deviceBll.SendCommandAsync(sendCommandDto);
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
        public async Task<DcpResponse<string>> DeleteRecordAsync(DateTime? fromDate , DateTime? toDate) => await deviceBll.DeleteRecordAsync(fromDate, toDate);

        [HttpGet]
        [Route("refresh/{companyId}")]
        public async Task<DcpResponse<string>> UpdateAppAsync(Guid companyId) => await deviceBll.SendCommandAsync(new SendCommandDto { Command = "silent_install" , CompanyId = companyId , IgnoreExecption = true , IsInternal = true , Token = [] });

        [HttpGet]
        [Route("check/{companyId}")]
        public async Task<DcpResponse<object>> CheckSettingsAsync(Guid companyId) => await deviceBll.CheckSettingsAsync(companyId);
        [HttpGet]
        [Route("getVersionCount/{companyId}")]
        public async Task<DcpResponse<int>> GetVersionCountAsync(Guid companyId) => await deviceBll.GetVersionCountAsync(companyId);


        [HttpPost]
        [Route("uploadImage")]
        public async Task<DcpResponse<bool>> UploadImageAsync([FromBody] ImageDto imageDto) => await deviceBll.UploadImageAndSendCommandAsync(imageDto);


    }

}
