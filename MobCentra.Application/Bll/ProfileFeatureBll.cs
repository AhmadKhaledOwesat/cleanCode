using MobCentra.Domain.Entities;
using MobCentra.Domain.Entities.Filters;
using MobCentra.Domain.Interfaces;

namespace MobCentra.Application.Bll
{
    public class ProfileFeatureBll(IBaseDal<ProfileFeature, Guid, ProfileFeatureFilter> baseDal) : BaseBll<ProfileFeature, Guid, ProfileFeatureFilter>(baseDal), IProfileFeatureBll
    {
        public override Task<PageResult<ProfileFeature>> GetAllAsync(ProfileFeatureFilter searchParameters)
        {
            searchParameters.Expression = new Func<ProfileFeature, bool>(a => a.ProfileId == searchParameters.ProfileId);
            return base.GetAllAsync(searchParameters);
        }

    }
}
