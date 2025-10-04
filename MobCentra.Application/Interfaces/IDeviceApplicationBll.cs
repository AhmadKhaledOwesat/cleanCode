using MobCentra.Application.Dto;
using MobCentra.Domain.Entities;
using MobCentra.Domain.Entities.Filters;

namespace MobCentra.Domain.Interfaces
{
    public interface IDeviceApplicationBll : IBaseBll<DeviceApplication, Guid, DeviceApplicationFilter>
    {
        Task<bool> UpdateStatus(DeviceBlockedApplicationDto[] entity);
    }
}
