using DcpTracker.Application.Dto;
using DcpTracker.Domain.Entities;
using DcpTracker.Domain.Entities.Filters;

namespace DcpTracker.Domain.Interfaces
{
    public interface IDeviceBll : IBaseBll<Device, Guid, DeviceFilter>
    {
        Task<DcpResponse<string>> SendCommandAsync(SendCommandDto sendCommandDto);
        Task<DcpResponse<string>> SendNotifyAsync(SendNotifyDto sendNotifyDto);
        Task<DcpResponse<string>> DeleteRecordAsync(DateTime? fromDate, DateTime? toDate);
        Task<DcpResponse<object>> CheckSettingsAsync(Guid companyId);
        Task<DcpResponse<int>> GetVersionCountAsync(Guid companyId);
        Task<DcpResponse<bool>> UploadImageAndSendCommandAsync(ImageDto imageDto);
    }
}
