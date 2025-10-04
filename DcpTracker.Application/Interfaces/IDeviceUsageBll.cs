using DcpTracker.Domain.Entities;
using DcpTracker.Domain.Entities.Filters;

namespace DcpTracker.Domain.Interfaces
{
    public interface IDeviceUsageBll : IBaseBll<DeviceUsage, Guid, DeviceUsageFilter>
    {
    }
}
