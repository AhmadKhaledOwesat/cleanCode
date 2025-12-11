using MobCentra.Domain.Entities;
using MobCentra.Domain.Entities.Filters;
using MobCentra.Domain.Interfaces;
using MobCentra.Infrastructure.Extensions;

namespace MobCentra.Application.Bll
{
    /// <summary>
    /// Business logic layer for setting management operations
    /// </summary>
    public class SettingBll(IBaseDal<Setting, Guid, SettingFilter> baseDal,Lazy<IDeviceBll> deviceBll) : BaseBll<Setting, Guid, SettingFilter>(baseDal), ISettingBll
    {
        /// <summary>
        /// Retrieves settings with filtering by company, keyword (setting name or display name), excluding system settings
        /// </summary>
        /// <param name="searchParameters">Filter parameters for searching and pagination</param>
        /// <returns>Paginated result containing matching settings</returns>
        public override async Task<PageResult<Setting>> GetAllAsync(SettingFilter searchParameters)
        {
            // Build search expression excluding system settings
            if (searchParameters is not null)
            {
                searchParameters.Expression = new Func<Setting, bool>(a => a.IsSystem == 0 && a.CompanyId == searchParameters.CompanyId
                && (searchParameters.Keyword.IsNullOrEmpty() || a.SettingName.Contains(searchParameters.Keyword) || a.DisplayName.Contains(searchParameters.Keyword))
                );
            }

            return await base.GetAllAsync(searchParameters);
        }

        /// <summary>
        /// Retrieves settings for a specific device by device code and optional setting name filter
        /// </summary>
        /// <param name="deviceCode">The device code to get settings for</param>
        /// <param name="settingName">Optional setting name to filter by</param>
        /// <returns>Paginated result containing matching settings for the device's company</returns>
        public async Task<PageResult<Setting>> GetSettingsAsync(string deviceCode,string settingName)
        {
            // Find device by code to get company ID
            var device = await deviceBll.Value.FindByExpressionAsync(a=>a.Code == deviceCode);
            return await GetAllAsync(new SettingFilter { CompanyId = device.CompanyId , Term = settingName });
        }

        public async Task<Setting> GetSettingByKeyAsync(string settingName)
        {
            return await FindByExpressionAsync(a=>a.SettingName == settingName);
        }
    }
}
