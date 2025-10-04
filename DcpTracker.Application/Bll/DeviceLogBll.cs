using DcpTracker.Domain.Entities;
using DcpTracker.Domain.Entities.Filters;
using DcpTracker.Domain.Interfaces;

namespace DcpTracker.Application.Bll
{
    public class DeviceLogBll(IBaseDal<DeviceLog, Guid, DeviceLogFilter> baseDal) : BaseBll<DeviceLog, Guid, DeviceLogFilter>(baseDal), IDeviceLogBll
    {
        public override Task<PageResult<DeviceLog>> GetAllAsync(DeviceLogFilter searchParameters)
        {
            if (searchParameters is not null)
            {
                if (!string.IsNullOrEmpty(searchParameters.Description))
                    searchParameters.Expression = new Func<DeviceLog, bool>(a => a.CommandName == searchParameters?.Description);
            }

            return base.GetAllAsync(searchParameters);
        }
    }
}
