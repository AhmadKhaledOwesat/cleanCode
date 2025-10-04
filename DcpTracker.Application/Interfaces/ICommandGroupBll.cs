using DcpTracker.Domain.Entities;
using DcpTracker.Domain.Entities.Filters;

namespace DcpTracker.Domain.Interfaces
{
    public interface ICommandGroupBll : IBaseBll<CommandGroup, Guid, CommandGroupFilter>
    {
    }
}
