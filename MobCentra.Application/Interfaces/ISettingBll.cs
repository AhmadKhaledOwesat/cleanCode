using MobCentra.Domain.Entities;
using MobCentra.Domain.Entities.Filters;

namespace MobCentra.Domain.Interfaces
{
    public interface ISettingBll : IBaseBll<Setting, Guid, SettingFilter>
    {
        Task<PageResult<Setting>> GetSettingsAsync(string deviceCode, string settingName);
        Task<Setting> GetSettingByKeyAsync(string settingName);
    }
}
