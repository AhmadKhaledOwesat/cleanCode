using DcpTracker.Application.Dto;
using DcpTracker.Domain.Entities;
using DcpTracker.Domain.Entities.Filters;

namespace DcpTracker.Domain.Interfaces
{
    public interface IDeviceApplicationBll : IBaseBll<DeviceApplication, Guid, DeviceApplicationFilter>
    {
        Task<bool> UpdateStatus(DeviceBlockedApplicationDto[] entity);
    }
}
