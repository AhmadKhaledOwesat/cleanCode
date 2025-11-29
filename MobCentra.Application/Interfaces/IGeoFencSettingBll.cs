using MobCentra.Domain.Entities;
using MobCentra.Domain.Entities.Filters;
using MobCentra.Domain.Interfaces;

namespace MobCentra.Application.Interfaces
{
    public interface IGeoFencSettingBll : IBaseBll<GeoFencSetting, Guid, GeoFencSettingFilter>
    {
    }
}
