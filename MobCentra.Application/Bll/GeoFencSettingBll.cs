using MobCentra.Application.Interfaces;
using MobCentra.Domain.Entities;
using MobCentra.Domain.Entities.Filters;
using MobCentra.Domain.Interfaces;

namespace MobCentra.Application.Bll
{
    public class GeoFencSettingBll(IBaseDal<GeoFencSetting, Guid, GeoFencSettingFilter> baseDal) : BaseBll<GeoFencSetting, Guid, GeoFencSettingFilter>(baseDal), IGeoFencSettingBll
    {
        public override Task<PageResult<GeoFencSetting>> GetAllAsync(GeoFencSettingFilter searchParameters)
        {
            searchParameters.Expression = new Func<GeoFencSetting, bool>(a => a.CompanyId == searchParameters?.CompanyId);
            return base.GetAllAsync(searchParameters);
        }
    }
}
