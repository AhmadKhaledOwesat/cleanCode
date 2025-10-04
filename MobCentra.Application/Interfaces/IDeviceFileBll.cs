using MobCentra.Domain.Entities.Filters;

namespace MobCentra.Domain.Interfaces
{
    public interface IDeviceFileBll : IBaseBll<Entities.DeviceFile, Guid, DeviceFileFilter>
    {
    }
}
