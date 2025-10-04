using MobCentra.Domain.Entities;
using MobCentra.Domain.Entities.Filters;

namespace MobCentra.Domain.Interfaces
{
    public interface IProfileBll : IBaseBll<Profile, Guid, ProfileFilter>
    {
        Task<Profile> GetProfileByDeviceIdAsync(Guid id);
    }
}
