using DcpTracker.Domain.Entities.Filters;

namespace DcpTracker.Domain.Interfaces
{
    public interface IDeviceFileBll : IBaseBll<Entities.DeviceFile, Guid, DeviceFileFilter>
    {
    }
}
