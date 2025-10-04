using DcpTracker.Domain.Entities.Filters;
using DeviceQueu = DcpTracker.Domain.Entities.DeviceQueu;

namespace DcpTracker.Domain.Interfaces
{
    public interface IDeviceQueuBll : IBaseBll<DeviceQueu, Guid, DeviceQueuFilter>
    {
    }
}
