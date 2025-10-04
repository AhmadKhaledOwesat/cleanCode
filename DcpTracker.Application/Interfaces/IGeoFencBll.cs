using DcpTracker.Domain.Entities.Filters;

namespace DcpTracker.Domain.Interfaces
{
    public interface IGeoFencBll : IBaseBll<Entities.GeoFenc, Guid, GeoFencFilter>
    {
    }
}
