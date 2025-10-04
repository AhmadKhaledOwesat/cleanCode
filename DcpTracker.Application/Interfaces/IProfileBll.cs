using DcpTracker.Domain.Entities;
using DcpTracker.Domain.Entities.Filters;

namespace DcpTracker.Domain.Interfaces
{
    public interface IProfileBll : IBaseBll<Profile, Guid, ProfileFilter>
    {
        Task<Profile> GetProfileByDeviceIdAsync(Guid id);
    }
}
