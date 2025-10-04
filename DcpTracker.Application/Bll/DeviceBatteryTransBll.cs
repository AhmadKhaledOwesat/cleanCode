using DcpTracker.Domain.Entities.Filters;
using DcpTracker.Domain.Interfaces;
using DcpTracker.Infrastructure.Extensions;
using DeviceBatteryTrans = DcpTracker.Domain.Entities.DeviceBatteryTrans;

namespace DcpTracker.Application.Bll
{
    public class DeviceBatteryTransBll(IBaseDal<DeviceBatteryTrans, Guid, DeviceBatteryTransFilter> baseDal) : BaseBll<DeviceBatteryTrans, Guid, DeviceBatteryTransFilter>(baseDal), IDeviceBatteryTransBll
    {
        public override Task<PageResult<DeviceBatteryTrans>> GetAllAsync(DeviceBatteryTransFilter searchParameters)
        {
            if (searchParameters is not null)
            {
                    searchParameters.Expression = new Func<DeviceBatteryTrans, bool>(a => 
                    
                    (a.DeviceId == searchParameters?.DeviceId) 
                  );
            }

            return base.GetAllAsync(searchParameters);
        }
    }
}
