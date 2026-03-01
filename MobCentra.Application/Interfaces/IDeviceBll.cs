using MobCentra.Application.Dto;
using MobCentra.Domain.Entities;
using MobCentra.Domain.Entities.Filters;

namespace MobCentra.Domain.Interfaces
{
    public interface IDeviceBll : IBaseBll<Device, Guid, DeviceFilter>
    {
        Task<DcpResponse<string>> SendCommandAsync(SendCommandDto sendCommandDto);
        Task<DcpResponse<string>> SendNotifyAsync(SendNotifyDto sendNotifyDto);
        Task<DcpResponse<string>> DeleteRecordAsync(DateTime? fromDate, DateTime? toDate);
        Task<DcpResponse<object>> CheckSettingsAsync(Guid companyId);
        Task<DcpResponse<dynamic>> GetVersionCountAsync(Guid companyId);
        Task<DcpResponse<bool>> UploadImageAndSendCommandAsync(ImageDto imageDto);
        Task<DcpResponse<bool>> UploadFileAndSendCommandAsync(ImageDto imageDto);

        Task<DcpResponse<bool>> HandleGeoFencCityAsync(List<GeoFencCityDto> geoFencCityDtos);
    }
}
