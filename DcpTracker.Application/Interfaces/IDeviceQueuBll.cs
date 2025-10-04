using MobCentra.Domain.Entities.Filters;
using DeviceQueu = MobCentra.Domain.Entities.DeviceQueu;

namespace MobCentra.Domain.Interfaces
{
    public interface IDeviceQueuBll : IBaseBll<DeviceQueu, Guid, DeviceQueuFilter>
    {
    }
}
