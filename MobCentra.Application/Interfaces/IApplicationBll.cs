using MobCentra.Domain.Entities.Filters;

namespace MobCentra.Domain.Interfaces
{
    public interface IApplicationBll : IBaseBll<Entities.Application, Guid, ApplicationFilter>
    {
    }
}
