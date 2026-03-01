using MobCentra.Domain.Entities;
using MobCentra.Domain.Entities.Filters;
using MobCentra.Domain.Interfaces;

namespace MobCentra.Application.Bll
{
    public class GeoFencBll(IBaseDal<GeoFenc, Guid, GeoFencFilter> baseDal) : BaseBll<GeoFenc, Guid, GeoFencFilter>(baseDal), IGeoFencBll
    {
        public override Task<PageResult<GeoFenc>> GetAllAsync(GeoFencFilter searchParameters)
        {
            searchParameters.Expression = new Func<GeoFenc, bool>(a => a.DeviceId == searchParameters.DeviceId);
            return base.GetAllAsync(searchParameters);
        }
        public override async Task AddAsync(GeoFenc entity)
        {
            await base.AddAsync(entity);
        }

        public override async Task<bool> DeleteAsync(Guid id)
        {
            var record = await FindByExpressionAsync(a => a.DeviceId == id);
            if (record is not null)
                await base.DeleteAsync(record.Id);

            return await Task.FromResult(true);
        }
    }
}
