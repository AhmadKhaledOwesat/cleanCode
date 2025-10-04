using MobCentra.Domain.Entities.Filters;

namespace MobCentra.Domain.Interfaces
{
    public interface IDeviceBatteryTransBll : IBaseBll<Entities.DeviceBatteryTrans, Guid, DeviceBatteryTransFilter>
    {
    }
}
