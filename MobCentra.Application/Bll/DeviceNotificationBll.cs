using MobCentra.Domain.Entities;
using MobCentra.Domain.Entities.Filters;
using MobCentra.Domain.Interfaces;
using MobCentra.Infrastructure.Extensions;
using System.Linq.Expressions;

namespace MobCentra.Application.Bll
{
    public class DeviceNotificationBll(IBaseDal<DeviceNotification, Guid, DeviceNotificationFilter> baseDal,IDeviceBll deviceBll) : BaseBll<DeviceNotification, Guid, DeviceNotificationFilter>(baseDal), IDeviceNotificationBll
    {
        public override Task<PageResult<DeviceNotification>> GetAllAsync(DeviceNotificationFilter searchParameters)
        {
            if (searchParameters is not null)
            {
                if (!string.IsNullOrEmpty(searchParameters.Description))
                    searchParameters.Expression = new Func<DeviceNotification, bool>(a => a.Location == searchParameters?.Description);
            }

            return base.GetAllAsync(searchParameters);
        }

        public override async Task AddAsync(DeviceNotification entity)
        {
            Expression<Func<Device,bool>> expression = x =>x.Code == entity.DeviceCode;
            Device device = await deviceBll.FindByExpressionAsync(expression);
            if(device != null)
            {
                entity.Device = null;
                entity.DeviceId = device.Id;
                if (!entity.ScreenShot.IsNullOrEmpty())
                    entity.ScreenShot = await entity.ScreenShot.UplodaFiles(".png", "images");
                await base.AddAsync(entity);
            }
        }
    }
}
