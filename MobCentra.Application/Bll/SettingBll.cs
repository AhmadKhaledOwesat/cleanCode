using MobCentra.Domain.Entities;
using MobCentra.Domain.Entities.Filters;
using MobCentra.Domain.Interfaces;
using MobCentra.Infrastructure.Extensions;

namespace MobCentra.Application.Bll
{
    public class SettingBll(IBaseDal<Setting, Guid, SettingFilter> baseDal,Lazy<IDeviceBll> deviceBll) : BaseBll<Setting, Guid, SettingFilter>(baseDal), ISettingBll
    {
        public override async Task<PageResult<Setting>> GetAllAsync(SettingFilter searchParameters)
        {

            if (searchParameters is not null)
            {
                searchParameters.Expression = new Func<Setting, bool>(a => a.IsSystem == 0 && a.CompanyId == searchParameters.CompanyId
                && (searchParameters.Term.IsNullOrEmpty() || a.SettingName.Contains(searchParameters.Term) )
                );
            }

            return await base.GetAllAsync(searchParameters);
        }

        public async Task<PageResult<Setting>> GetSettingsAsync(string deviceCode,string settingName)
        {
            var device = await deviceBll.Value.FindByExpressionAsync(a=>a.Code == deviceCode);
            return await GetAllAsync(new SettingFilter { CompanyId = device.CompanyId , Term = settingName });
        }
    }
}
