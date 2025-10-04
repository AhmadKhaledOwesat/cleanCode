using DcpTracker.Domain.Entities.Filters;

namespace DcpTracker.Domain.Interfaces
{
    public interface IDeviceBatteryTransBll : IBaseBll<Entities.DeviceBatteryTrans, Guid, DeviceBatteryTransFilter>
    {
    }
}
