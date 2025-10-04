using MobCentra.Domain.Entities;
using MobCentra.Domain.Entities.Filters;

namespace MobCentra.Domain.Interfaces
{
    public interface IDeviceNotificationBll : IBaseBll<DeviceNotification, Guid, DeviceNotificationFilter>
    {
    }
}
