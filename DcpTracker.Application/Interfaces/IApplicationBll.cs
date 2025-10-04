using DcpTracker.Domain.Entities.Filters;

namespace DcpTracker.Domain.Interfaces
{
    public interface IApplicationBll : IBaseBll<Entities.Application, Guid, ApplicationFilter>
    {
    }
}
