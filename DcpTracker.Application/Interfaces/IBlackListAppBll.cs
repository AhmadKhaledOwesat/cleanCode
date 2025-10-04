using DcpTracker.Domain.Entities;
using DcpTracker.Domain.Entities.Filters;
namespace DcpTracker.Domain.Interfaces
{
    public interface IBlackListAppBll : IBaseBll<BlackListApp, Guid, BlackListAppFilter>
    {
    }
}
