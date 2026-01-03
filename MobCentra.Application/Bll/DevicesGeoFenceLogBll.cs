using MobCentra.Domain.Entities;
using MobCentra.Domain.Entities.Filters;
using MobCentra.Domain.Interfaces;

namespace MobCentra.Application.Bll
{
    public class DevicesGeoFenceLogBll(IBaseDal<DevicesGeoFenceLog, Guid, DevicesGeoFenceLogFilter> baseDal) : BaseBll<DevicesGeoFenceLog, Guid, DevicesGeoFenceLogFilter>(baseDal), IDevicesGeoFenceLogBll
    {
    }
}
