using MobCentra.Domain.Entities;
using MobCentra.Domain.Entities.Filters;
using MobCentra.Domain.Interfaces;

namespace MobCentra.Application.Bll
{
    public class ProfileApplicationBll(IBaseDal<ProfileApplication, Guid, ProfileApplicationFilter> baseDal) : BaseBll<ProfileApplication, Guid, ProfileApplicationFilter>(baseDal), IProfileApplicationBll
    {
        public override Task<PageResult<ProfileApplication>> GetAllAsync(ProfileApplicationFilter searchParameters)
        {
            searchParameters.Expression = new Func<ProfileApplication, bool>(a => a.ProfileId == searchParameters.ProfileId);
            return base.GetAllAsync(searchParameters);
        }

    }
}
