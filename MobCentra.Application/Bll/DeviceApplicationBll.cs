using MobCentra.Application.Dto;
using MobCentra.Domain.Entities;
using MobCentra.Domain.Entities.Filters;
using MobCentra.Domain.Interfaces;
using System.Linq.Expressions;

namespace MobCentra.Application.Bll
{
    public class DeviceApplicationBll(IBaseDal<DeviceApplication, Guid, DeviceApplicationFilter> baseDal,IDeviceBll deviceBll) : BaseBll<DeviceApplication, Guid, DeviceApplicationFilter>(baseDal), IDeviceApplicationBll
    {
        public override Task<PageResult<DeviceApplication>> GetAllAsync(DeviceApplicationFilter searchParameters)
        {
            if(searchParameters.DeviceId.HasValue && searchParameters.DeviceId != Guid.Empty)
            searchParameters.Expression = new Func<DeviceApplication, bool>(a => a.DeviceId == searchParameters?.DeviceId);
            return base.GetAllAsync(searchParameters);
        }
        public async Task<bool> DeleteAsync(string packageName,Guid deviceId)
        {
            var app =  await FindByExpressionAsync(a=>a.PackgeName.Equals(packageName) && a.DeviceId == deviceId);
            if(app != null)
            {
                await base.DeleteAsync(app.Id);
            }
            return true;
        }
        public async Task<bool> UpdateStatus(DeviceBlockedApplicationDto[] entity)
        {

            foreach (var item in entity)
            {
                DeviceApplication deviceApplication = await base.GetByIdAsync(item.Id);
                if (deviceApplication != null)
                {
                    deviceApplication.IsBlocked = item.IsBlocked;
                    await base.UpdateAsync(deviceApplication);
                }
            }
            return true;
        }
        public override async Task AddAsync(DeviceApplication entity)
        {
            Device device = await deviceBll.FindByExpressionAsync(ex => ex.Code == entity.Code);
            entity.DeviceId = device.Id;
            entity.Device = null;
            DeviceApplication application = await base.FindByExpressionAsync(ex => ex.DeviceId == device.Id && ex.PackgeName == entity.PackgeName);
            if (application != null)
            {

                entity.Id = application.Id;
                entity.CreatedDate = application.CreatedDate;
                entity.CreatedBy = application.CreatedBy;
                entity.IsBlocked = application.IsBlocked;
                await base.UpdateAsync(entity);
            }
            else
            {
                await base.AddAsync(entity);
            }
        }
        public override async Task AddRangeAsync(List<DeviceApplication> entities)
        {
            var apps = await FindAllByExpressionAsync(ex => ex.DeviceId == entities.First().DeviceId);
            if (apps.Count != 0) await DeleteRangeAsync(apps);
            await base.AddRangeAsync(entities);
        }

    }
}
