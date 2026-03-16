using MobCentra.Domain.Entities;
using MobCentra.Domain.Entities.Filters;

namespace MobCentra.Domain.Interfaces
{
    public interface IProfileFeatureBll : IBaseBll<ProfileFeature, Guid, ProfileFeatureFilter>
    {
    }

    public interface IProfileApplicationBll : IBaseBll<ProfileApplication, Guid, ProfileApplicationFilter>
    {
    }
}
