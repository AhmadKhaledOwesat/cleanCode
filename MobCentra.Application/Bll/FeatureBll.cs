using MobCentra.Domain.Entities.Filters;
using MobCentra.Domain.Interfaces;
using MobCentra.Infrastructure.Extensions;
using Feature = MobCentra.Domain.Entities.Feature;

namespace MobCentra.Application.Bll
{
    public class FeatureBll(IBaseDal<Feature, int, FeatureFilter> baseDal) : BaseBll<Feature, int, FeatureFilter>(baseDal), IFeatureBll
    {
        public override Task<PageResult<Feature>> GetAllAsync(FeatureFilter searchParameters)
        {
            if (searchParameters is not null)
            {
                    searchParameters.Expression = new Func<Feature, bool>(a =>
                    (searchParameters.Keyword.IsNullOrEmpty() || a.NameAr.Contains(searchParameters?.Keyword))
                   && a.Active == 1);
            }

            return base.GetAllAsync(searchParameters);
        }
    }
}
