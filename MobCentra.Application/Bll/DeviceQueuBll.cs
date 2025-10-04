using MobCentra.Domain.Entities;
using MobCentra.Domain.Entities.Filters;
using MobCentra.Domain.Interfaces;
using MobCentra.Infrastructure.Extensions;

namespace MobCentra.Application.Bll
{
    public class DeviceQueuBll(IBaseDal<DeviceQueu, Guid, DeviceQueuFilter> baseDal) : BaseBll<DeviceQueu, Guid, DeviceQueuFilter>(baseDal), IDeviceQueuBll
    {
    }
}
