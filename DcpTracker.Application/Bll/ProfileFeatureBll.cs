using DcpTracker.Domain.Entities;
using DcpTracker.Domain.Entities.Filters;
using DcpTracker.Domain.Interfaces;

namespace DcpTracker.Application.Bll
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
