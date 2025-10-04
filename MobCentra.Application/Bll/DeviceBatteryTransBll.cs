using MobCentra.Domain.Entities.Filters;
using MobCentra.Domain.Interfaces;
using MobCentra.Infrastructure.Extensions;
using DeviceBatteryTrans = MobCentra.Domain.Entities.DeviceBatteryTrans;

namespace MobCentra.Application.Bll
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
