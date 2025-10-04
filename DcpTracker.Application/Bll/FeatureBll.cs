using DcpTracker.Domain.Entities.Filters;
using DcpTracker.Domain.Interfaces;
using DcpTracker.Infrastructure.Extensions;
using Feature = DcpTracker.Domain.Entities.Feature;

namespace DcpTracker.Application.Bll
{
    public class FeatureBll(IBaseDal<Feature, int, FeatureFilter> baseDal) : BaseBll<Feature, int, FeatureFilter>(baseDal), IFeatureBll
    {
        public override Task<PageResult<Feature>> GetAllAsync(FeatureFilter searchParameters)
        {
            if (searchParameters is not null)
            {
                    searchParameters.Expression = new Func<Feature, bool>(a => 
                    (a.NameAr == searchParameters?.Description || searchParameters.Description.IsNullOrEmpty()) 
                   && a.Active == 1);
            }

            return base.GetAllAsync(searchParameters);
        }
    }
}
