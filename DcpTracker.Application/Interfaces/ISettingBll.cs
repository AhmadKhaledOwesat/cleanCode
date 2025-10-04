using DcpTracker.Domain.Entities;
using DcpTracker.Domain.Entities.Filters;

namespace DcpTracker.Domain.Interfaces
{
    public interface ISettingBll : IBaseBll<Setting, Guid, SettingFilter>
    {
        Task<PageResult<Setting>> GetSettingsAsync(string deviceCode, string settingName);
    }
}
