using MobCentra.Domain.Entities.Filters;

namespace MobCentra.Domain.Interfaces
{
    public interface IGeoFencBll : IBaseBll<Entities.GeoFenc, Guid, GeoFencFilter>
    {
    }
}
