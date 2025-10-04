using DcpTracker.Domain.Entities;
using DcpTracker.Domain.Entities.Filters;
using DcpTracker.Domain.Interfaces;
using DcpTracker.Infrastructure.Extensions;

namespace DcpTracker.Application.Bll
{
    public class DeviceUsageBll(IBaseDal<DeviceUsage, Guid, DeviceUsageFilter> baseDal, IDeviceBll deviceBll) : BaseBll<DeviceUsage, Guid, DeviceUsageFilter>(baseDal), IDeviceUsageBll
    {
        public override async Task<PageResult<DeviceUsage>> GetAllAsync(DeviceUsageFilter searchParameters)
        {
            if (searchParameters is not null)
            {
                searchParameters.Expression = new Func<DeviceUsage, bool>(a =>
                (a.DeviceId == searchParameters?.DeviceId)
                && (a.FromDate >= searchParameters.FromDate || !searchParameters.FromDate.HasValue)
                && (a.ToDate <= searchParameters.ToDate || !searchParameters.ToDate.HasValue)
                 && (a.AppName.Contains(searchParameters.AppName) || searchParameters.AppName.IsNullOrEmpty()));
            }
            return await base.GetAllAsync(searchParameters);
        }
        public override async Task AddAsync(DeviceUsage entity)
        {
            entity.DeviceId = (await deviceBll.FindByExpressionAsync(a => a.Code == entity.DeviceCode)).Id;
            await base.AddAsync(entity);
        }
        public override async Task AddRangeAsync(List<DeviceUsage> entities)
        {
            foreach (var entity in entities) {
                entity.DeviceId = (await deviceBll.FindByExpressionAsync(a => a.Code == entity.DeviceCode)).Id;
            }
             await base.AddRangeAsync(entities);
        }
     
    }
}
