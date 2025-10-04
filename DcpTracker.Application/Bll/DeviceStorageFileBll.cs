using MobCentra.Domain.Entities.Filters;
using MobCentra.Domain.Interfaces;
using MobCentra.Domain.Entities;
using MobCentra.Infrastructure.Extensions;
namespace MobCentra.Application.Bll
{
    public class DeviceStorageFileBll(IBaseDal<DeviceStorageFile, Guid, DeviceStorageFileFilter> baseDal, IDeviceBll deviceBll) : BaseBll<DeviceStorageFile, Guid, DeviceStorageFileFilter>(baseDal), IDeviceStorageFileBll
    {
        public override async Task<PageResult<DeviceStorageFile>> GetAllAsync(DeviceStorageFileFilter searchParameters)
        {
            if (searchParameters is not null)
            {
                searchParameters.Expression = new Func<DeviceStorageFile, bool>(a =>
                 a.CompanyId == searchParameters.CompanyId
                && (searchParameters.DeviceId == searchParameters.DeviceId));
            }
            return await base.GetAllAsync(searchParameters);
        }

        public override async Task AddAsync(DeviceStorageFile entity)
        {
            await AddRangeAsync([entity]);
        }

        public override async Task AddRangeAsync(List<DeviceStorageFile> entities)
        {
            foreach (var entity in entities)
            {
                var device = await deviceBll.FindByExpressionAsync(a => a.Code == entity.DeviceCode);
                if (device == null) continue;

                entity.DeviceId = device.Id;
                entity.CompanyId = device.CompanyId;
                if (!entity.Path.IsNullOrEmpty())
                    entity.Path = await entity.Path.UplodaFiles($".{entity.Extension}",name: entity.Name.Split(".")[0]);
            }
            await base.AddRangeAsync(entities);
        }
    }
}
