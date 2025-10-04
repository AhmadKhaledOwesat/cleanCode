using MobCentra.Domain.Entities.Filters;
using MobCentra.Domain.Interfaces;
using MobCentra.Infrastructure.Extensions;
using Version = MobCentra.Domain.Entities.Version;

namespace MobCentra.Application.Bll
{
    public class VersionBll(IBaseDal<Version, Guid, VersionFilter> baseDal) : BaseBll<Version, Guid, VersionFilter>(baseDal), IVersionBll
    {
        public override Task<PageResult<Version>> GetAllAsync(VersionFilter searchParameters)
        {
            if (searchParameters is not null)
            {
                if (!searchParameters.Name.IsNullOrEmpty())
                    searchParameters.Expression = new Func<Version, bool>(a => a.VersionName == searchParameters?.Name);
            }

            return base.GetAllAsync(searchParameters);
        }
    }
}
