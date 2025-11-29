using MobCentra.Domain.Entities;
using MobCentra.Domain.Entities.Filters;
using MobCentra.Domain.Interfaces;
namespace MobCentra.Application.Bll
{
    public class DeviceFileBll(IBaseDal<DeviceFile, Guid, DeviceFileFilter> baseDal, IDeviceBll deviceBll) : BaseBll<DeviceFile, Guid, DeviceFileFilter>(baseDal), IDeviceFileBll
    {
        public override async Task<PageResult<DeviceFile>> GetAllAsync(DeviceFileFilter searchParameters)
        {
            return await base.GetAllAsync(searchParameters);
        }

        public override async Task AddRangeAsync(List<DeviceFile> entities)
        {
            if (entities.Count != 0)
            {
                var device = await deviceBll.FindByExpressionAsync(a => a.Code == entities[0].DeviceCode);
                var dbData = await FindAllByExpressionAsync(a => a.DeviceId == device.Id);
                await base.DeleteRangeAsync(dbData);
            }

            foreach (var entity in entities)
            {
                var device = await deviceBll.FindByExpressionAsync(a => a.Code == entity.DeviceCode);
                if (device == null) continue;

                entity.DeviceId = device.Id;
                entity.CompanyId = device.CompanyId;
            }
            await base.AddRangeAsync(entities);
        }
    }
}
