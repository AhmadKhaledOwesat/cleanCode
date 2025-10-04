using DcpTracker.Domain.Entities;
using DcpTracker.Domain.Entities.Filters;
using City = DcpTracker.Domain.Entities.City;

namespace DcpTracker.Domain.Interfaces
{
    public interface ICityBll : IBaseBll<City, Guid, CityFilter>
    {
    }
}
