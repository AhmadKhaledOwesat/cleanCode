using DcpTracker.Domain.Entities;
using DcpTracker.Domain.Entities.Filters;
using DcpTracker.Domain.Interfaces;
using DcpTracker.Infrastructure.Extensions;

namespace DcpTracker.Application.Bll
{
    public class DeviceQueuBll(IBaseDal<DeviceQueu, Guid, DeviceQueuFilter> baseDal) : BaseBll<DeviceQueu, Guid, DeviceQueuFilter>(baseDal), IDeviceQueuBll
    {
    }
}
