using MobCentra.Domain.Entities;
using MobCentra.Domain.Entities.Filters;
using City = MobCentra.Domain.Entities.City;

namespace MobCentra.Domain.Interfaces
{
    public interface ICityBll : IBaseBll<City, Guid, CityFilter>
    {
    }
}
