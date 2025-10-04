using MobCentra.Domain.Entities.Filters;
using Version = MobCentra.Domain.Entities.Version;

namespace MobCentra.Domain.Interfaces
{
    public interface IVersionBll : IBaseBll<Version, Guid, VersionFilter>
    {
    }
}
