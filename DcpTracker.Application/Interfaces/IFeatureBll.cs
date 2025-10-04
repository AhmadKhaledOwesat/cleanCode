using DcpTracker.Domain.Entities;
using DcpTracker.Domain.Entities.Filters;

namespace DcpTracker.Domain.Interfaces
{
    public interface IFeatureBll : IBaseBll<Feature, int, FeatureFilter>
    {
    }
}
