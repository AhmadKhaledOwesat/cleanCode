using DcpTracker.Domain.Entities.Filters;
using Version = DcpTracker.Domain.Entities.Version;

namespace DcpTracker.Domain.Interfaces
{
    public interface IVersionBll : IBaseBll<Version, Guid, VersionFilter>
    {
    }
}
